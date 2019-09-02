using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Heroes.Hint.Library;
using Heroes.Hint.Library.Structure;
using Heroes.Hint.Library.Structure.Heroes.Enum;
using Reloaded.WPF.MVVM;

namespace Heroes.Hint.Editor.Model
{
    public class MainPageViewModel : ObservableObject
    {
        public string CurrentIndexText => $"Current Index: {CurrentEntryIndex}/{Entries.Count}";
        public int CurrentEntryIndex   => Entries.IndexOf(CurrentEntry);

        /// <summary>
        /// Stores the location of the currently opened file.
        /// </summary>
        public string CurrentFileLocation { get; set; }
        
        /// <summary>
        /// List of modifiable entries.
        /// </summary>
        public ObservableCollection<ManagedEntry> Entries { get; set; } = new ObservableCollection<ManagedEntry>();

        /// <summary>
        /// The item currently being modified.
        /// </summary>
        public ManagedEntry CurrentEntry { get; set; }

        /// <summary>
        /// Gets/sets the text which decides which entries will be shown in the GUI.
        /// </summary>
        public string TextFilter { get; set; }

        /* Methods */

        public void AddNewEntry()
        {
            short maxHintNumber = Entries.Count <= 0 ?
                (short)-1 :
                (short)Entries.Max(x => x.HintNumber);

            var newEntry = new ManagedEntry((short)(maxHintNumber + 1), Character.Sonic, "Reloaded II is Cool!", 1337, 0);
            Entries.Add(newEntry);
            CurrentEntry = newEntry;
        }

        public void RemoveCurrentEntry()
        {
            int indexOfCurrent = Entries.IndexOf(CurrentEntry);
            
            // Set next hint to 0 where it was pointing to current index.
            // For all indexes above this one, reduce by 1.
            foreach (var entry in Entries)
            {
                if (entry.NextHint == indexOfCurrent)
                    entry.NextHint = 0;
                else if (entry.NextHint > indexOfCurrent)
                    entry.NextHint = (short) (entry.NextHint - 1);
            } 

            Entries.Remove(CurrentEntry);
        }

        public void OpenFile(string filePath)
        {
            Entries = new ObservableCollection<ManagedEntry>(HintFile.FromArray(File.ReadAllBytes(filePath)).Entries);
            CurrentFileLocation = filePath;
            if (Entries.Count > 0)
                CurrentEntry = Entries[0];
        }

        public void SaveFile(string filePath) => File.WriteAllBytes(filePath, new HintFile(Entries.ToArray()).ToArray());
        public void SaveCurrentPath()
        {
            if (! String.IsNullOrEmpty(CurrentFileLocation))
                SaveFile(CurrentFileLocation);
        }
    }
}
