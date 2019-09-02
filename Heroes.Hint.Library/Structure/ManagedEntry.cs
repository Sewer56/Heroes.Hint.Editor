using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Heroes.Hint.Library.Annotations;
using Heroes.Hint.Library.Structure.Heroes;
using Heroes.Hint.Library.Structure.Heroes.Enum;

namespace Heroes.Hint.Library.Structure
{
    public class ManagedEntry : INotifyPropertyChanged
    {
        /// <summary>
        /// Number of the hint which matches the number in the object layout file.
        /// </summary>
        public short HintNumber         { get; set; }

        /// <summary>
        /// The character which triggers this hint.
        /// </summary>
        public Character Character      { get; set; }

        /// <summary>
        /// The text belonging to this hint. Inlined and ending in null terminator.
        /// </summary>
        public string Text              { get; set; }

        /// <summary>
        /// Amount of frames the hint is shown.
        /// </summary>
        public short ShowDuration       { get; set; }

        /// <summary>
        /// Index of the next hint to play after this hint completes.
        /// The index is the order it appears in the final file.
        /// </summary>
        public short NextHint           { get; set; }

        public ManagedEntry(short hintNumber, Character character, string text, short showDuration, short nextHint) : this()
        {
            HintNumber = hintNumber;
            Character = character;
            Text = text;
            ShowDuration = showDuration;
            NextHint = nextHint;
        }

        public ManagedEntry(Entry entry, string text) : this()
        {
            HintNumber = entry.HintNumber;
            Character = entry.Character;
            ShowDuration = entry.ShowDuration;
            NextHint = entry.NextHint;
            Text = text;
        }

        private ManagedEntry()
        {
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AsString) && (e.PropertyName == nameof(HintNumber) || 
                                                       e.PropertyName == nameof(Character) || 
                                                       e.PropertyName == nameof(Text)))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AsString)));
        }
        
        /* Overrides & PropertyChanged */

        /// <summary> Shortcut for ToString to use with Data Binding. </summary>
        public string AsString => ToString();
        public override string ToString()
        {
            return $"{HintNumber}-{Character}: {Text}";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
