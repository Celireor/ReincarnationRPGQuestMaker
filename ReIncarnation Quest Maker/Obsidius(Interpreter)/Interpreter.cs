using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;

namespace ReIncarnation_Quest_Maker.Obsidius
{
    public static class Interpreter
    {
        public static MainForm ThisForm;

        public static QuestList CurrentQuestList;

        public static Quest SelectedQuest;

        public static QuestStage SelectedQuestStage;

        static Interpreter() {
           // GenerateDefaultQuestList();
        }

        public static void GenerateDefaultQuestList() {
            ClearScreen();
            CurrentQuestList = new QuestList();
            AddQuest();
            //CurrentQuestList.Quests.Add(SelectedQuest);

            CurrentQuestList.ThisEditorExternal.LargestQuestID = 1;

            SelectedQuest.name = "New Quest";
            SelectedQuest.full_name = "new_quest_internal_name";
            SelectedQuest.description = "Quest Description";
            SelectedQuest.portrait = "questgiver_portrait";
            SelectedQuest.typeIcon = "story";
            SelectedQuest.questID = 1;
            SelectedQuest.cantAbandon = true;
            SelectedQuest.repeatable = false;

            ThisForm.LoadValues();
            UpdateScreen();
        }

        //Select functions

        public static void SelectQuest(Quest SelectedQuest) {
            Interpreter.SelectedQuest = SelectedQuest;
            Interpreter.SelectedQuestStage = Interpreter.SelectedQuest.stages[0];
            ThisForm.UpdateQuestData();
            UpdateScreen();
        }

        public static void SelectQuestStage(QuestStage SelectedQuestStage) {
            Interpreter.SelectedQuestStage = SelectedQuestStage;
            ThisForm.UpdateStageData();
            UpdateScreen();
        }

        //Add Functions

        public static void AddQuest()
        {
            CurrentQuestList.ThisEditorExternal.LargestQuestID++;
            Quest NewQuest = Quest.GenerateDefaultQuest(CurrentQuestList.ThisEditorExternal.LargestQuestID);
            SelectQuest(NewQuest);
            CurrentQuestList.Quests.Add(NewQuest);

            ThisForm.OnNewQuest(NewQuest);

            UpdateScreen();
        }

        public static void AddQuestStage() {
            QuestStage NewStage = QuestStage.Generate(SelectedQuest.stages.Count, SelectedQuest);
            SelectQuestStage(NewStage);

            ThisForm.OnNewQuestStage(NewStage);
        }

        public static void AddPrerequisite()
        {
            KVPair NewPrerequisite = new KVPair();
            SelectedQuest.prerequisites.Add(NewPrerequisite);
            ThisForm.OnNewPrerequisite(NewPrerequisite);
        }

        public static void AddAffectedNPC()
        {
            //QuestInjectDialogue NewAffectedNpc = QuestInjectDialogue.Generate(SelectedQuestStage);

            //ThisForm.OnNewAffectedNPC(NewAffectedNpc);
        }

        public static void AddQuestDialogue()
        {
            QuestDialogue NewDialogue = QuestDialogue.Generate(SelectedQuest);
            ThisForm.OnNewQuestDialogue(NewDialogue);
        }

        //others

        public static void AddQuestStageTask(string TaskType)
        {
            QuestTask NewStage = QuestTask.Generate(TaskType, SelectedQuestStage);

            ThisForm.OnNewQuestStageTask(NewStage);
        }

        public static void AddQuestStageDialogue ()
        {
            KVPair NewStageDialogue = new KVPair();
            SelectedQuestStage.dialogue.Add(NewStageDialogue);
            ThisForm.OnNewQuestStageDialogue(NewStageDialogue);
        }

        public static void LoadNewQuestList(QuestList NewQuestList, string FilePath)
        {
            ClearScreen();
            NewQuestList.ThisEditorExternal.FilePath = FilePath;
            NewQuestList.ThisEditorExternal.PossibleListenStrings = CurrentQuestList.ThisEditorExternal.PossibleListenStrings;
            CurrentQuestList = NewQuestList;

            CurrentQuestList.SortQuests();
            GetLargestQuestID();

            SelectQuest(CurrentQuestList.Quests[0]);

            ThisForm.LoadValues();
            UpdateScreen();

            CurrentQuestList.Quests.ForEach(obj => { ThisForm.OnNewQuest(obj); });
        }

        public static void Print(string FilePath)
        {
            CurrentQuestList.SortQuests();
            CurrentQuestList.ThisEditorExternal.FilePath = FilePath;
            string Data = CurrentQuestList.ConvertToText();
            Unbidden.Write(FilePath, Data);
        }

        public static void GetLargestQuestID()
        {
            CurrentQuestList.SortQuests();
            CurrentQuestList.ThisEditorExternal.LargestQuestID = CurrentQuestList.Quests[CurrentQuestList.Quests.Count - 1].questID;
        }

        //Update

        public static void UpdateScreen() { ThisForm.UpdateScreen(); }

        public static void ClearScreen() { ThisForm.ClearScreen(); }

        /* Basic quest value changes */

        public static void UpdateSelectedQuestName(string NewValue)
        {
            SelectedQuest.name = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }
        public static void UpdateSelectedQuestInternalName(string NewValue)
        {
            SelectedQuest.full_name = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestPortrait(string NewValue)
        {
            SelectedQuest.portrait = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestDescription(string NewValue)
        {
            SelectedQuest.description = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestCantAbandon(bool NewValue)
        {
            SelectedQuest.cantAbandon = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestRepeatable(bool NewValue)
        {
            SelectedQuest.repeatable = NewValue;
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestID(int NewValue)
        {
            if (NewValue == SelectedQuest.questID || NewValue < 0) {
                return;
            }
            bool Occupied = false;
            CurrentQuestList.Quests.ForEach(obj => {
                Occupied |= obj.questID == NewValue && obj != SelectedQuest;
            });
            if (Occupied == true)
            {
                UpdateScreen();
                return;
            }
            if (NewValue > CurrentQuestList.ThisEditorExternal.LargestQuestID)
            {
                CurrentQuestList.ThisEditorExternal.LargestQuestID = NewValue;
            }
            int OldID = SelectedQuest.questID;
            SelectedQuest.SetQuestID (NewValue);
            if (OldID == CurrentQuestList.ThisEditorExternal.LargestQuestID) {
                GetLargestQuestID();
            }
            SelectedQuest.ThisEditorExternal.OnUpdate();
        }

        public static void UpdateSelectedQuestIcon(string NewValue)
        {
            SelectedQuest.typeIcon = NewValue;
        }

        public static void UpdateSelectedQuestStateDescription(string NewValue)
        {
            SelectedQuestStage.description = NewValue;
        }

        /* END */
    }
}
