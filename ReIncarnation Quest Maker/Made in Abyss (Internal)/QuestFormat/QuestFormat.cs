﻿using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{
    public class QuestList : QuestVariable
    {
        public QuestList_EditorExternal ThisEditorExternal = new QuestList_EditorExternal();

        public List<Quest> Quests = new List<Quest>();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnString = "";
            Quests.ForEach(obj => { ReturnString += obj.ConvertToText(TabCount + 2); });
            return EncapsulateInQuestContainer(ReturnString, TabCount);
        }

        public string EncapsulateInQuestContainer(string inputstring, int TabCount = 0)
        {
            return PrintEncapsulation(PrintEncapsulation(inputstring, TabCount + 1, "Quests", true), TabCount, "Quests", true);

        }

        public override void GenerateFromKV(KVPair ThisKV)
        {

            KVPairFolder ThisKVPairFolder = ThisKV.FolderValue;
            ThisKVPairFolder.Items.ForEach(obj =>
            {
                Quest NewQuest = Quest.KVGenerate(obj, this);
                Quests.Add(NewQuest);
                ThisEditorExternal.PossibleTypeIcons.Add(NewQuest.typeIcon);
            });

            ThisEditorExternal.StringsToFix.ForEach(obj => {
                ThisEditorExternal.AllTalkTos.ForEach(obj2 => {
                    if (obj.ThisOptionalFields.sendListenString == obj2.listenString) {
                        obj2.listenString = obj.ThisEditorExternal.Parent.questID.ToString() + '_' + obj2.listenString;
                    }
                });
            });
        }

        public void MergeQuestList(QuestList OtherList) {
            List<Quest> OtherListReverse = new List<Quest>();

            OtherList.Quests.ForEach(obj =>
            {
                OtherListReverse.Insert(0, obj);
            });

            OtherListReverse.ForEach(obj => {
                obj.UpdateQuestID(ThisEditorExternal.LargestQuestID + obj.questID);
                obj.ThisEditorExternal.ParentQuestList = this;
            });

            Quests.AddRange(OtherList.Quests);
            ThisEditorExternal.PossibleListenStrings.AddRange(OtherList.ThisEditorExternal.PossibleListenStrings);

            GetLargestQuestID();
        }

        public Quest CloneQuest(Quest QuestToClone) {
            string TextConverted = QuestToClone.ConvertToText(2);
            TextConverted = EncapsulateInQuestContainer(TextConverted);
            QuestList NewQuestList = QuestFormatParser.Parse(TextConverted, "");
            Quest ReturnValue = NewQuestList.Quests[0];
            ReturnValue.ForceSetQuestID(1);
            MergeQuestList(NewQuestList);
            return ReturnValue;
        }

        public void SortQuests()
        {
            Quests.Sort(Quest.SortByQuestID());
        }

        public Quest GetQuest(int ID) {
            Quest ReturnValue = null;
            Quests.ForEach(obj => { if (obj.questID == ID) { ReturnValue = obj; } });
            return ReturnValue;
        }

        public void GetLargestQuestID()
        {
            SortQuests();
            ThisEditorExternal.LargestQuestID = Quests[Quests.Count - 1].questID;
        }

        public class QuestList_EditorExternal : QuestVariableEditorExternal
        {

            public string FilePath;
            public int LargestQuestID;
            public ListeningList<string> PossibleTypeIcons = new ListeningList<string>();
            public ListeningList<string> PossibleQuestStates = new ListeningList<string>();
            public ListeningList<string> PossibleQuestOptionSelectImage = new ListeningList<string>();
            public ListeningList<string> PossibleParticles = new ListeningList<string>();

            public ListeningList<string> PossibleListenStrings = new ListeningList<string>();

            public ListeningList<string> PossibleHideIfStrings = new ListeningList<string>();

            public List<QuestTask_talkto> AllTalkTos = new List<QuestTask_talkto>();
            public List<QuestDialogueOption> StringsToFix = new List<QuestDialogueOption>();

            public QuestList_EditorExternal()
            {

                PossibleTypeIcons.Add("story");
                PossibleTypeIcons.Add("class");
                PossibleTypeIcons.Add("repeatable");

                PossibleQuestStates.Add("true");
                PossibleQuestStates.Add("false");

                PossibleQuestOptionSelectImage.Add("story");
                PossibleQuestOptionSelectImage.Add("story_repeatable");
                PossibleQuestOptionSelectImage.Add("check");
                PossibleQuestOptionSelectImage.Add("chat");

                PossibleParticles.Add("quest_return");
                PossibleParticles.Add("quest_start");
                PossibleParticles.Add("chat");

                PossibleHideIfStrings.Add("quests");
                PossibleHideIfStrings.Add("prereqs");
                PossibleHideIfStrings.Add("items");
            }
        }
    }

    public class Quest : QuestVariable
    {

        //public string full_name;
        public string name;
        public int questID { get { return ThisEditorExternal.questID; } set { ThisEditorExternal.questID = value; } }
        public string description;
        public bool repeatable;
        public string portrait;
        public string typeIcon;
        public bool cantAbandon;

        public KVList questprerequisites = new KVList();

        public List<QuestStage> stages = new List<QuestStage>();

        public List<QuestDialogue> injections = new List<QuestDialogue>();

        public Quest_EditorExternal ThisEditorExternal = new Quest_EditorExternal();

        public void ForceSetQuestID(int NewValue) {

            questID = NewValue;
            ThisEditorExternal.OnUpdate();
        }

        public static Quest GenerateDefaultQuest(int questID, QuestList Parent)
        {
            Quest ReturnValue = new Quest();
            ReturnValue.ThisEditorExternal.ParentQuestList = Parent;
            ReturnValue.ForceSetQuestID(questID);
            ReturnValue.typeIcon = "story";
            QuestStage.Generate(0, ReturnValue);

            return ReturnValue;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            /* Contents of Quest */
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);

            /* Contents of Affected NPCs */

            List<string> AffectedNPCNames = new List<string>();
            string StagesString = QuestStage.ConvertToText_Full(stages, AffectedNPCNames, TabCount + 1);
            string AffectedNPCString = "";
            int x = 0;
            AffectedNPCNames.Distinct().ToList().ForEach(obj =>
            {
                x++;
                AffectedNPCString += PrintKeyValue(x.ToString(), obj, TabCount + 2);
            });
            StringToEncapsulate += PrintEncapsulation(AffectedNPCString, TabCount + 1, "affectedNpcs", true);

            /* Contents of Prerequisites */

            {
                string prereqstring = "";
                if (ThisEditorExternal.prereqlevel > 0)
                {
                    prereqstring += PrintKeyValue("level", ThisEditorExternal.prereqlevel.ToString(), TabCount + 2);
                }
                if (ThisEditorExternal.prereqclass != "" && ThisEditorExternal.prereqclass != null)
                {
                    prereqstring += PrintKeyValue("class", ThisEditorExternal.prereqclass, TabCount + 2);
                }
                if (questprerequisites.Count > 0) {
                    string questprerequisitesstring = "";
                    questprerequisites.ForEach(obj => questprerequisitesstring += obj.ConvertToText(TabCount + 3));
                    prereqstring += PrintEncapsulation(questprerequisitesstring, TabCount + 2, "quests", true);
                }
                if(prereqstring != "")
                {
                    StringToEncapsulate += PrintEncapsulation(prereqstring, TabCount + 1, "prerequisites", true);
                }
            }

            /* Contents of Stages */
            StringToEncapsulate += StagesString;

            /* Contents of injections */
            string injectionsString = "";
            injections.ForEach(obj => {
                injectionsString += obj.ConvertToText(TabCount + 2);
            });
            StringToEncapsulate += PrintEncapsulation(injectionsString, TabCount + 1, "injections", true);

            /* Encapsulate everything */

            return PrintEncapsulation(StringToEncapsulate, TabCount, questID.ToString(), true);
        }

        public static Quest KVGenerate(KVPair ThisKV, QuestList Parent) {
            Quest ReturnValue = GenerateDefaultQuest(0, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisFieldInfo) =>
            {
                switch (IterationKV.Key)
                {
                    case "prerequisites":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj => {
                                switch (obj.Key)
                                {
                                    case "level":
                                        {
                                            ThisEditorExternal.prereqlevel = Convert.ToInt32(obj.Value);
                                        }
                                        break;
                                    case "class":
                                        {
                                            ThisEditorExternal.prereqclass = obj.Value;
                                        }
                                        break;
                                    case "quests":
                                        {
                                            questprerequisites = new KVList(obj.FolderValue.Items);
                                        }
                                        break;
                                }
                            });
                            //prerequisites = new KVList(IterationKV.FolderValue.Items);
                        }
                        break;
                    /*case "affectedNpcs":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                //QuestInjectDialogue newAffectedNPC = QuestInjectDialogue.GenerateFromKV_Parent(obj, this);
                            });
                        }
                        break;*/
                    case "stages":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                QuestStage.KVGenerate(obj, this);
                            });
                        }
                        break;
                    case "injections":
                        {
                            IterationKV.FolderValue.Items.ForEach((obj) =>
                            {
                                QuestDialogue.KVGenerate(obj, this);
                            });
                        }
                        break;
                }
            });
            questID = Convert.ToInt32(ThisKV.Key);
        }

        public void UpdateQuestID(int NewValue)
        {
            if (NewValue == questID || NewValue < 0)
            {
                return;
            }
            bool Occupied = false;
            ThisEditorExternal.ParentQuestList.Quests.ForEach(obj => {
                Occupied |= obj.questID == NewValue && obj != this;
            });
            if (Occupied == true)
            {
                return;
            }
            if (NewValue > ThisEditorExternal.ParentQuestList.ThisEditorExternal.LargestQuestID)
            {
                ThisEditorExternal.ParentQuestList.ThisEditorExternal.LargestQuestID = NewValue;
            }
            int OldID = questID;
            ForceSetQuestID(NewValue);
            if (OldID == ThisEditorExternal.ParentQuestList.ThisEditorExternal.LargestQuestID)
            {
                ThisEditorExternal.ParentQuestList.GetLargestQuestID();
            }
            ThisEditorExternal.OnUpdate();
        }

        public static IComparer<Quest> SortByQuestID()
        {
            return new QuestIDComparer();
        }

        private class QuestIDComparer : IComparer<Quest>
        {
            int IComparer<Quest>.Compare(Quest x, Quest y)
            {
                return x.questID.CompareTo(y.questID);
            }
        }

        public class Quest_EditorExternal : QuestVariableEditorExternal
        {
            public QuestList ParentQuestList;
            public int questID;

            public ListeningList<String> PossibleDialogueInjections = new ListeningList<string>();

            public int prereqlevel;
            public string prereqclass;
        }
    }

    public class QuestStage : QuestVariable
    {
        //public List<QuestInjectDialogue> affectedNPCs = new List<QuestInjectDialogue>();
        public QuestStage_OptionalFields ThisOptionalFields {
            get
            {
                return OptionalFields as QuestStage_OptionalFields;
            }
        }

        public List<QuestTask> tasks = new List<QuestTask>();
        public KVList dialogue = new KVList();
        public KVList particles = new KVList();

        public QuestStage_EditorExternal ThisEditorExternal = new QuestStage_EditorExternal();

        public QuestStage() {
            OptionalFields = new QuestStage_OptionalFields();
        }

        public override bool Trash()
        {
            ThisEditorExternal.Parent.stages.Remove(this);
            ThisEditorExternal.Parent.stages.AssignIndexValuesToListItems((obj, index) =>
            {
                obj.ThisEditorExternal.StageNum = index;
                obj.ThisEditorExternal.OnUpdate();
            });
            return true;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            //dontuse.
            throw new NotImplementedException();
        }

        public static void GetRewardsValues(KVPair KV, QuestStage ReturnValue) {
            switch (KV.Key)
            {
                case "gold":
                    {
                        ReturnValue.ThisEditorExternal.gold = Convert.ToInt32(KV.Value);
                    }
                    break;
                case "exp":
                    {
                        ReturnValue.ThisEditorExternal.exp = Convert.ToInt32(KV.Value);
                    }
                    break;
                case "items":
                    {
                        ReturnValue.ThisEditorExternal.items = new KVList(KV.FolderValue.Items);
                    }
                    break;
            }
        }

        public static QuestStage KVGenerate(KVPair ThisKV, Quest Parent)
        {
            int Num = Convert.ToInt32(ThisKV.Key);
            QuestStage ReturnValue = Insert(Num, Parent);
            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisField) =>
            {
                switch (IterationKV.Key)
                {
                    case "onCompletion":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj => {
                                switch (obj.Key)
                                {
                                    case "quests":
                                        {
                                            ReturnValue.ThisEditorExternal.QuestsGiven = new KVList(obj.FolderValue.Items);
                                        }
                                        break;
                                    case "rewards":
                                        {
                                            obj.FolderValue.Items.ForEach(obj2 => {
                                                GetRewardsValues(obj2, ReturnValue);
                                            });
                                        }
                                        break;
                                    case "consume":
                                        {
                                            ReturnValue.ThisEditorExternal.itemsConsumed = new KVList(obj.FolderValue.Items);
                                        }
                                        break;
                                }
                                GetRewardsValues(obj, ReturnValue);
                            });
                        }
                        break;
                    //opens the injections folder
                    case "inject":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj =>
                            {
                                switch (obj.Key)
                                {
                                    case "dialogue":
                                        {
                                            obj.FolderValue.Items.ForEach(obj2 =>
                                            {
                                                ReturnValue.dialogue.Add(GenerateFromKeyValue<KVPair>(obj2));
                                            });
                                        }
                                        break;
                                    case "particle":
                                        {
                                            obj.FolderValue.Items.ForEach(obj2 =>
                                            {
                                                ReturnValue.particles.Add(GenerateFromKeyValue<KVPair>(obj2));
                                            });
                                        }
                                        break;
                                }
                            });
                        }
                        break;
                    //opens the tasks folder
                    case "tasks":
                        {
                            IterationKV.FolderValue.Items.ForEach(obj =>
                            {
                                QuestTask.MassGenerate<QuestTask_location>(ReturnValue, obj, "location");
                                QuestTask.MassGenerate<QuestTask_gather>(ReturnValue, obj, "gather");
                                QuestTask.MassGenerate<QuestTask_kill>(ReturnValue, obj, "kill");
                                QuestTask.MassGenerate<QuestTask_killType>(ReturnValue, obj, "killType");
                                QuestTask.MassGenerate<QuestTask_talkto>(ReturnValue, obj, "talkto");
                                QuestTask.MassGenerate<QuestTask_event>(ReturnValue, obj, "event");
                                QuestTask.MassGenerate<QuestTask_useAbility>(ReturnValue, obj, "useAbility");
                                QuestTask.MassGenerate<QuestTask_reachLevel>(ReturnValue, obj, "reachLevel");
                            });
                        }
                        break;
                }
            });
            return ReturnValue;
        }

        public static QuestStage Generate(int StageNum, Quest Parent)
        {
            QuestStage ReturnValue = new QuestStage();
            ReturnValue.ThisEditorExternal.StageNum = StageNum;
            ReturnValue.ThisEditorExternal.Parent = Parent;
            Parent.stages.Add(ReturnValue);
            return ReturnValue;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string stringtoencapsulate = ConvertToText_Iterate(TabCount + 1);

            string OnCompletionString = "";

            if (ThisEditorExternal.QuestsGiven.Count > 0)
            {
                OnCompletionString += PrintEncapsulation(ThisEditorExternal.QuestsGiven.ConvertToText(TabCount + 3), TabCount + 2, "quests", true);
            }

            if (ThisEditorExternal.itemsConsumed.Count > 0)
            {
                OnCompletionString += PrintEncapsulation(ThisEditorExternal.itemsConsumed.ConvertToText(TabCount + 3), TabCount + 2, "consume", true);
            }

            if (ThisEditorExternal.gold > 0 || ThisEditorExternal.exp > 0 || ThisEditorExternal.items.Count > 0) {
                string goldxpstring = PrintKeyValue("gold", ThisEditorExternal.gold.ToString(), TabCount + 3);
                goldxpstring += PrintKeyValue("exp", ThisEditorExternal.exp.ToString(), TabCount + 3);

                if (ThisEditorExternal.items.Count > 0)
                {
                    string itemsrewardstring = "";
                    ThisEditorExternal.items.ForEach(obj => itemsrewardstring += obj.ConvertToText(TabCount + 4));
                    goldxpstring += PrintEncapsulation(itemsrewardstring, TabCount + 3, "items", true);
                }

                OnCompletionString += PrintEncapsulation(goldxpstring, TabCount + 2, "rewards", true);
            }

            if (OnCompletionString != "") {
                stringtoencapsulate += PrintEncapsulation(OnCompletionString, TabCount + 1, "onCompletion", true);
            }

            string InjectionsString = "";

            if (dialogue.Count > 0)
            {
                string DialogueString = "";
                dialogue.ForEach(obj => DialogueString += obj.ConvertToText(TabCount + 3));
                InjectionsString += PrintEncapsulation(DialogueString, TabCount + 2, "dialogue", true);
            }
            if (particles.Count > 0)
            {
                string ParticleString = "";
                particles.ForEach(obj => ParticleString += obj.ConvertToText(TabCount + 3));
                InjectionsString += PrintEncapsulation(ParticleString, TabCount + 2, "particle", true);
            }

            if (InjectionsString != "") {
                stringtoencapsulate += PrintEncapsulation(InjectionsString, TabCount + 1, "inject", true);
            }

            if (tasks.Count > 0)
            {

                QuestIndexableVariableStringConverterLibrary<QuestTask> TaskLibrary = new QuestIndexableVariableStringConverterLibrary<QuestTask>(
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_gather), "gather"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_location), "location"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_talkto), "talkto"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_kill), "kill"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_killType), "killType"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_event), "event"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_useAbility), "useAbility"),
                    new QuestIndexableVariableStringConverter<QuestTask>(typeof(QuestTask_reachLevel), "reachLevel")
                );
                stringtoencapsulate += TaskLibrary.ConvertToText(tasks, "tasks", TabCount);
            }

            return PrintEncapsulation(stringtoencapsulate, TabCount, Convert.ToString(ThisEditorExternal.StageNum), true);
        }

        public static string ConvertToText_Full(List<QuestStage> Group, List<string>AffectedNPCNames, int TabCount = 0)
        {
            string ReturnValue = "";

            Group.ForEach(obj =>
            {
                obj.dialogue.ForEach(obj2 => AffectedNPCNames.Add(obj2.Key));
                obj.particles.ForEach(obj2 => AffectedNPCNames.Add(obj2.Key));
                ReturnValue += obj.ConvertToText(TabCount + 1);
            });

            return PrintEncapsulation(ReturnValue, TabCount, "stages", true);
        }

        public static QuestStage Insert(int StageNum, Quest Parent)
        {
            QuestStage ReturnValue = Parent.stages.Find(a => a.ThisEditorExternal.StageNum == StageNum); ;
            if (ReturnValue == null)
            {
                ReturnValue = Generate(StageNum, Parent);
            }
            return ReturnValue;
        }

        public class QuestStage_EditorExternal : QuestVariableEditorExternal
        {
            public Quest Parent;
            public int StageNum;

            //rewards
            public int gold;
            public int exp;
            public KVList items = new KVList();

            public KVList itemsConsumed = new KVList();

            public KVList QuestsGiven = new KVList();
        }

        public class QuestStage_OptionalFields : QuestVariableOptionalFields
        {
            public string description;
        }
    }

    public class QuestDialogue : QuestVariable
    {
        public ListeningList<QuestDialogueOption> options = new ListeningList<QuestDialogueOption>();

        public QuestDialogue_EditorExternal ThisEditorExternal = new QuestDialogue_EditorExternal();

        public override bool Trash() {
            ThisEditorExternal.ThisQuest.injections.Remove(this);
            ThisEditorExternal.ForceDisableName();
            return true;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";
            int x = 0;
            options.ForEach(obj =>
            {
                x++;
                ReturnValue += PrintEncapsulation(obj.ConvertToText(TabCount + 2), TabCount + 1, x.ToString(), true);
            });
            if (ReturnValue != "") {
                ReturnValue = PrintEncapsulation(ReturnValue, TabCount, ThisEditorExternal.DialogueName, true);
            }
            return ReturnValue;
        }

        public static QuestDialogue Generate(Quest Parent) {

            QuestDialogue ReturnValue = new QuestDialogue();
            ReturnValue.ThisEditorExternal.ThisQuest = Parent;
            ReturnValue.options.AddListener(ReturnValue.ThisEditorExternal.EnableName);

            Parent.injections.Add(ReturnValue);
            return ReturnValue;
        }

        public static QuestDialogue KVGenerate(KVPair ThisKV, Quest Parent) {
            QuestDialogue ReturnValue = Generate(Parent);
            ReturnValue.ThisEditorExternal.ChangeName(ThisKV.Key);
            ThisKV.FolderValue.Items.ForEach(obj => QuestDialogueOption.KVGenerate(obj, Parent, ReturnValue.options));

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public class QuestDialogue_EditorExternal : QuestVariableEditorExternal
        {
            public string DialogueName = "";
            public Quest ThisQuest;
            bool IsEmpty = true;
            public void ChangeName(string NewName)
            {
                if (!IsEmpty)
                {
                    ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Remove(DialogueName);
                }
                DialogueName = NewName;
                if (!IsEmpty)
                {
                    ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Add(DialogueName);
                }
            }

            public void EnableName(ListeningList<QuestDialogueOption> ThisList, QuestDialogueOption UselessField = null)
            {

                if (!IsEmpty && ThisList.Count == 0)
                {
                    ForceDisableName();
                }

                else if (IsEmpty && ThisList.Count > 0)
                {
                    ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Add(DialogueName);
                    IsEmpty = false;
                }
            }

            public void ForceDisableName() {

                ThisQuest.ThisEditorExternal.PossibleDialogueInjections.Remove(DialogueName);
                IsEmpty = true;
            }
        }
    }

    public class QuestDialogueOption : QuestVariable
    {

        public string selectText;
        public string selectImg;

        public List<QuestDialogueOptionRunScript> runScript = new List<QuestDialogueOptionRunScript>();

        public KVList giveQuests = new KVList();

        public List<QuestDialogueOptionHideIf> hideIf = new List<QuestDialogueOptionHideIf>();

        public QuestDialogueResponse Response;

        //public List<QuestDialogueOption_hideIf> hideIf = new List<QuestDialogueOption_hideIf>();

        public QuestDialogueOption_OptionalFields ThisOptionalFields {
            get { return (QuestDialogueOption_OptionalFields)OptionalFields; }
        }

        public QuestDialogueOption() {
            OptionalFields = new QuestDialogueOption_OptionalFields();
        }

        public override bool Trash() {
            ThisEditorExternal.ParentList.Remove(this);
            return true;
        }

        public QuestDialogueOption_EditorExternal ThisEditorExternal = new QuestDialogueOption_EditorExternal();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";
            if (hideIf.Count > 0) {

                QuestIndexableVariableStringConverterLibrary<QuestDialogueOptionHideIf> QuestHideIfList = new QuestIndexableVariableStringConverterLibrary<QuestDialogueOptionHideIf> (
                    new QuestIndexableVariableStringConverter<QuestDialogueOptionHideIf>(typeof(QuestDialogueOptionHideIf_Quest), "quests"),
                    new QuestIndexableVariableStringConverter<QuestDialogueOptionHideIf>(typeof(QuestDialogueOptionHideIf_Prereqs), "prereqs")
                );

                ReturnValue += QuestHideIfList.ConvertToText(hideIf, "hideIf", TabCount);
            }

            string templistenstring = ThisOptionalFields.sendListenString;
            if(templistenstring != "")
            {
                ThisOptionalFields.sendListenString = Convert.ToString(ThisEditorExternal.Parent.questID) + "_" + ThisOptionalFields.sendListenString;
            }

            ReturnValue += ConvertToText_Iterate(TabCount);

            ThisOptionalFields.sendListenString = templistenstring;

            /*if (HasConverted)
            {
                ReturnValue += new string('\t', TabCount) + "\"" + "listeningQuest" + "\"\t\"" + ThisEditorExternal.listeningQuest + "\"" + Environment.NewLine; ;
            }*/

            if (giveQuests.Count > 0) {
                string giveQuestsString = "";
                giveQuests.ForEach(obj => {
                    giveQuestsString += obj.ConvertToText(TabCount + 1);
                });
                ReturnValue += PrintEncapsulation(giveQuestsString, TabCount, "giveQuests", true);
            }

            string gotoString = "";

            runScript.ForEach(obj => gotoString += runScript[0].ConvertToText(TabCount + 1));
            //todo: upgrade to support multiple scripts once jobo rework format

            if (Response != null) {
                gotoString += Response.ConvertToText(TabCount + 1);
            }

            if (gotoString != null)
            {
                ReturnValue += PrintEncapsulation(gotoString, TabCount, "goto", true);
            }

            return ReturnValue;
        }

        public static QuestDialogueOption Generate(Quest Parent, ListeningList<QuestDialogueOption> ParentList) {
            QuestDialogueOption ReturnValue = new QuestDialogueOption();
            ReturnValue.ThisEditorExternal.Parent = Parent;
            ReturnValue.ThisEditorExternal.ParentList = ParentList;
            ReturnValue.ThisOptionalFields.ThisEditorExternal.Parent = Parent;
            ReturnValue.ThisOptionalFields.ThisEditorExternal.LastID = Parent.questID;
            Parent.ThisEditorExternal.OnUpdateList.Add(ReturnValue.ThisOptionalFields.ChangeID);
            ParentList.Add(ReturnValue);
            return ReturnValue;
        }

        public static QuestDialogueOption KVGenerate(KVPair ThisKV, Quest Parent, ListeningList<QuestDialogueOption> ParentList) {
            QuestDialogueOption ReturnValue = Generate(Parent, ParentList);

            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (obj, objType) => {
                switch (obj.Key)
                {
                    case "hideIf":
                        {
                            obj.FolderValue.Items.ForEach(obj2 => {
                                QuestDialogueOptionHideIf.MassGenerate<QuestDialogueOptionHideIf_Quest>(ReturnValue, obj2, "quests");
                                QuestDialogueOptionHideIf.MassGenerate<QuestDialogueOptionHideIf_Prereqs>(ReturnValue, obj2, "prereqs");
                            });
                        }
                        break;
                    case "goto":
                        {

                            ReturnValue.Response = QuestDialogueResponse.KVGenerate(obj, Parent, ReturnValue);
                        }
                        break;
                    case "giveQuest":
                        {
                            ReturnValue.giveQuests.Add(new KVPair(obj.Value, "false"));
                        }
                        break;
                    case "giveQuests":
                        {
                            obj.FolderValue.Items.ForEach(obj2 => {
                                ReturnValue.giveQuests.Add(GenerateFromKeyValue<KVPair>(obj2));
                                });
                        }
                        break;
                    case "runScript":
                        {
                            ReturnValue.runScript.Add(GenerateFromKeyValue<QuestDialogueOptionRunScript>(obj));
                        }
                        break;
                }
                /*if (obj.Key == "listeningQuest")
                {
                    ReturnValue.ThisEditorExternal.listeningQuest = Convert.ToInt32(obj.Value);
                }*/
            });
            if (ReturnValue.ThisOptionalFields.sendListenString != "") {
                int NumToRemove = QuestDialogueOption_OptionalFields.GetQuestIDFromListenString(ReturnValue.ThisOptionalFields.sendListenString);
                if (NumToRemove == 0) {
                    Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.StringsToFix.Add(ReturnValue);
                }
                else
                {
                    ReturnValue.ThisOptionalFields.ChangeListenString(ReturnValue.ThisOptionalFields.sendListenString.Remove(0, NumToRemove));
                }
            }

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public class QuestDialogueOption_EditorExternal : QuestVariableEditorExternal
        {
            public bool AutoGenerateListenStringID = true;
            public Quest Parent;
            public ListeningList<QuestDialogueOption> ParentList;

            //public int listeningQuest;
        }
    }

    public class QuestDialogueOption_OptionalFields : QuestVariableOptionalFields
    {
        //public string questMarker = "";
        //public string selectBackground = "";
        public string sendListenString = "";

        public QuestDialogueOption_OptionalFields_EditorExternal ThisEditorExternal = new QuestDialogueOption_OptionalFields_EditorExternal();

        public void ChangeListenString(string sendListenString)
        {
            if (this.sendListenString != "")
            {
                ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.PossibleListenStrings.Remove(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
            }
            this.sendListenString = sendListenString;
            if (this.sendListenString != "")
            {
                ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.PossibleListenStrings.Add(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
            }
        }

        public static int GetQuestIDFromListenString(string ListenString)
        {
            string TestIDString = "";
            for (int x = 0; x < ListenString.Length; x++)
            {
                if (ListenString[x] == '_')
                {
                    int result;
                    if (int.TryParse(TestIDString, out result))
                    {
                        return x + 1;
                    }
                    return 0;
                }
                TestIDString += ListenString[x];
            }
            return 0;
        }

        public void ChangeID()
        {
            if (this.sendListenString != "")
            {
                ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.PossibleListenStrings.Remove(ThisEditorExternal.LastID + "_" + this.sendListenString);
                ThisEditorExternal.Parent.ThisEditorExternal.ParentQuestList.ThisEditorExternal.PossibleListenStrings.Add(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
                ThisEditorExternal.LastID = ThisEditorExternal.Parent.questID;
            }
        }

        public class QuestDialogueOption_OptionalFields_EditorExternal : QuestVariableEditorExternal
        {
            public Quest Parent;
            public int LastID;
        }
    }

    public class QuestDialogueResponse : QuestVariable
    {

        public string responseText = "";
        public ListeningList<QuestDialogueOption> options = new ListeningList<QuestDialogueOption>();
        public KVList QuestsToGive = new KVList();
        public QuestDialogueResponse_OptionalFields ThisOptionalFields
        {
            get
            {
                return OptionalFields as QuestDialogueResponse_OptionalFields;
            }
        }

        public QuestDialogueResponse() {
            OptionalFields = new QuestDialogueResponse_OptionalFields();
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "";

            ReturnValue += ConvertToText_Iterate(TabCount);

            if (options.Count > 0)
            {
                string OptionsString = "";
                int x = 0;

                options.ForEach(obj =>
                {
                    x++;
                    OptionsString += PrintEncapsulation(obj.ConvertToText(TabCount + 2), TabCount + 1, x.ToString(), true);
                });

                ReturnValue += PrintEncapsulation(OptionsString, TabCount, "options", true);
            }

            return ReturnValue;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            throw new NotImplementedException();
        }

        public static QuestDialogueResponse KVGenerate(KVPair ThisKV, Quest Parent, QuestDialogueOption ThisOption)
        {
            QuestDialogueResponse ReturnValue = new QuestDialogueResponse();

            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (obj, objType) => {
                switch (obj.Key)
                {
                    case "options":
                        {
                            obj.FolderValue.Items.ForEach(obj2 => {
                                QuestDialogueOption.KVGenerate(obj2, Parent, ReturnValue.options);
                            });
                        }
                        break;
                    case "sendListenString":
                        {
                            ThisOption.ThisOptionalFields.ChangeListenString(obj.Value);
                        }
                        break;
                }
                
            });

            return ReturnValue;
        }

        public class QuestDialogueResponse_OptionalFields : QuestVariableOptionalFields
        {
            public string responsePortait = "";
            public string playSound = "";
        }
    }

    public class QuestDialogueOptionRunScript : QuestVariable
    {
        public string className;
        public string functionName;
        public KVList arguments = new KVList ();

        public override string ConvertToText(int TabCount = 0)
        {
            string StringToEncapsulate = ConvertToText_Iterate(TabCount + 1);
            StringToEncapsulate += PrintEncapsulation(arguments.ConvertToText(TabCount + 2), TabCount + 1, "arguments", true);
            return PrintEncapsulation(StringToEncapsulate, TabCount, "runScript", true);
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (obj, info) => {
            if (obj.Key == "arguments") {
                   arguments = new KVList(obj.FolderValue.Items);
                }
            });
        }
    }
}
