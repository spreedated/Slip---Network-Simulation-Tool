using Microsoft.Extensions.Logging;
using NHotkey;
using NHotkey.Wpf;
using System;
using System.Windows.Input;

namespace Slip.Logic
{
    public class Hotkey : IDisposable
    {
        private bool disposedValue;
        private readonly ILogger logger = null;

        public Key HotkeyKey { get; private set; } = Key.None;
        public ModifierKeys Modifierkeys { get; private set; }

        /// <summary>
        /// The unique name of the hotkey.
        /// </summary>
        public string Name { get; init; }

        public bool IsEnabled { get; private set; }

        public int PressedCount { get; private set; }

        public Action HotkeyAction { get; init; }

        public bool IsHotkeyAssigned
        {
            get
            {
                return this.HotkeyKey != Key.None;
            }
        }

        #region Constructor
        public Hotkey(string name, Action hotkeyAction, ILogger logger = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            ArgumentNullException.ThrowIfNull(hotkeyAction);

            this.Name = name;
            this.HotkeyAction = hotkeyAction;
            this.logger = logger;
        }
        #endregion

        private static bool IsModifierKey(Key key)
        {
            return key == Key.LeftCtrl || key == Key.RightCtrl ||
                   key == Key.LeftAlt || key == Key.RightAlt ||
                   key == Key.LeftShift || key == Key.RightShift ||
                   key == Key.LWin || key == Key.RWin ||
                   key == Key.System;
        }

        private void HotkeyPressed(object sender, HotkeyEventArgs e)
        {
            this.logger?.LogTrace("[{Name}] Hotkey pressed", this.Name);
            this.HotkeyAction.Invoke();
            this.PressedCount++;
            e.Handled = true;
        }

        /// <summary>
        /// Assign a new Combination to the hotkey.<br/>
        /// This will delete the current combination.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        public void AssignHotkeyCombination(Key key, ModifierKeys modifierKeys = ModifierKeys.None)
        {
            this.HotkeyKey = key;
            this.Modifierkeys = modifierKeys;

            this.logger?.LogTrace("[{Name}] Hotkey combination assigned: {Combination}", this.Name, this.ToString());
        }

        public void AppendKeyToHotkeyCombination(Key key)
        {
            if (!IsModifierKey(key))
            {
                this.HotkeyKey = key;

                this.logger?.LogTrace("[{Name}] Hotkey combination assigned: {Combination}", this.Name, this.ToString());
                return;
            }

            if ((key == Key.LeftCtrl || key == Key.RightCtrl) && !this.Modifierkeys.HasFlag(ModifierKeys.Control))
            {
                this.Modifierkeys |= ModifierKeys.Control;
                this.logger?.LogTrace("[{Name}] Hotkey modfier appended {ModKey}", this.Name, ModifierKeys.Control);
            }

            if ((key == Key.LeftAlt || key == Key.RightAlt || key == Key.System) && (!this.Modifierkeys.HasFlag(ModifierKeys.Alt)))
            {
                this.Modifierkeys |= ModifierKeys.Alt;
                this.logger?.LogTrace("[{Name}] Hotkey modfier appended {ModKey}", this.Name, ModifierKeys.Alt);
            }

            if ((key == Key.LeftShift || key == Key.RightShift) && !this.Modifierkeys.HasFlag(ModifierKeys.Shift))
            {
                this.Modifierkeys |= ModifierKeys.Shift;
                this.logger?.LogTrace("[{Name}] Hotkey modfier appended {ModKey}", this.Name, ModifierKeys.Shift);
            }

            if ((key == Key.LWin || key == Key.RWin) && !this.Modifierkeys.HasFlag(ModifierKeys.Windows))
            {
                this.Modifierkeys |= ModifierKeys.Windows;
                this.logger?.LogTrace("[{Name}] Hotkey modfier appended {ModKey}", this.Name, ModifierKeys.Windows);
            }
        }

        /// <summary>
        /// Will clear the hotkey combination and disable the hotkey.<br/>
        /// Will not reset the pressed count.
        /// </summary>
        public void Clear()
        {
            this.Deactivate();
            this.HotkeyKey = Key.None;
            this.Modifierkeys = ModifierKeys.None;
            this.IsEnabled = false;

            this.logger?.LogTrace("[{Name}] Hotkey cleared", this.Name);
        }

        public void Activate()
        {
            if (this.IsEnabled)
            {
                this.logger?.LogTrace("[{Name}] Hotkey already enabled", this.Name);
                return;
            }

            if (this.HotkeyKey == Key.None)
            {
                throw new InvalidOperationException("The hotkey key is not set.");
            }

            HotkeyManager.Current.AddOrReplace(this.Name, this.HotkeyKey, this.Modifierkeys, this.HotkeyPressed);

            this.IsEnabled = true;

            this.logger?.LogTrace("[{Name}] Hotkey enabled", this.Name);
        }

        public void Deactivate()
        {
            if (!this.IsEnabled)
            {
                this.logger?.LogTrace("[{Name}] Hotkey already disabled", this.Name);
                return;
            }

            HotkeyManager.Current.Remove(this.Name);
            this.IsEnabled = false;

            this.logger?.LogTrace("[{Name}] Hotkey disabled", this.Name);
        }

        public void Toggle()
        {
            if (this.IsEnabled)
            {
                this.Deactivate();
                return;
            }

            this.Activate();
        }

        public override string ToString()
        {
            return $"{this.Modifierkeys} + {this.HotkeyKey}";
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Deactivate();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
