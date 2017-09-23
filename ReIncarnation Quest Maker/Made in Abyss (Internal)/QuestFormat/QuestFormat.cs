using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat
{

    public class QuestList : QuestVariable
    {
        public QuestList_EditorExternal ThisEditorExternal = new QuestList_EditorExternal();

        public List<Quest> Quests = new List<Quest>();

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnString = "";
            Quests.ForEach(obj => { ReturnString += obj.ConvertToText(); });
            return ReturnString;
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {

            KVPairFolder ThisKVPairFolder = ThisKV.FolderValue;
            ThisKVPairFolder.Items.ForEach(obj =>
            {
                Quest NewQuest = GenerateFromKeyValue<Quest>(obj);
                Quests.Add(NewQuest);
                ThisEditorExternal.PossibleTypeIcons.Add(NewQuest.typeIcon);
            });

        }

        public void SortQuests()
        {
            Quests.Sort(Quest.SortByQuestID());
        }

        public class QuestList_EditorExternal : QuestVariableEditorExternal
        {

            public string FilePath;
            public int LargestQuestID;
            public ListeningList<string> PossibleTypeIcons = new ListeningList<string>();
            public ListeningList<string> PossibleQuestPrerequisites = new ListeningList<string>();
            public ListeningList<string> PossibleQuestOptionSelectImage = new ListeningList<string>();
            public ListeningList<string> PossibleParticles = new ListeningList<string>();

            public ListeningList<string> PossibleListenStrings = new ListeningList<string>();


            public QuestList_EditorExternal()
            {

                PossibleTypeIcons.Add("story");
                PossibleTypeIcons.Add("class");
                PossibleTypeIcons.Add("repeatable");

                PossibleQuestPrerequisites.Add("level");

                PossibleQuestOptionSelectImage.Add("story");
                PossibleQuestOptionSelectImage.Add("story_repeatable");
                PossibleQuestOptionSelectImage.Add("check");
                PossibleQuestOptionSelectImage.Add("chat");

                PossibleParticles.Add("quest_return");
                PossibleParticles.Add("quest_start");
                PossibleParticles.Add("chat");
            }
        }
    }

    public class Quest : QuestVariable
    {

        public string full_name;
        public string name;
        public string description;
        public bool repeatable;
        public int questID;
        public string portrait;
        public string typeIcon;
        public bool cantAbandon;

        public KVList prerequisites = new KVList();

        public List<QuestStage> stages = new List<QuestStage>();

        public List<QuestDialogue> injections = new List<QuestDialogue>();

        public Quest_EditorExternal ThisEditorExternal = new Quest_EditorExternal();

        public void SetQuestID(int NewValue) {

            questID = NewValue;
            ThisEditorExternal.OnUpdate();
        }

        public static Quest GenerateDefaultQuest(int questID)
        {
            Quest ReturnValue = new Quest();
            ReturnValue.SetQuestID(questID);
            ReturnValue.typeIcon = "story";
            QuestStage.Generate(0, ReturnValue);

            return ReturnValue;
        }

        public override string ConvertToText(int TabCount = 0)
        {
            string ReturnValue = "\"" + questID + "\"" + Environment.NewLine;

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
                AffectedNPCString += new string('\t', TabCount + 2) + "\"" + x.ToString() + "\"\t\"" + obj + "\"" + Environment.NewLine;
            });
            StringToEncapsulate += PrintEncapsulation(AffectedNPCString, TabCount + 1, "affectedNpcs", true);

            /* Contents of Prerequisites */
            StringToEncapsulate += PrintListVariables(prerequisites, "prerequisites", TabCount + 1);

            /* Contents of Stages */
            StringToEncapsulate += StagesString;

            /* Contents of injections */
            string injectionsString = "";
            injections.ForEach(obj => {
                injectionsString += obj.ConvertToText(TabCount + 2);
            });
            StringToEncapsulate += PrintEncapsulation(injectionsString, TabCount + 1, "injections", true);

            /* Encapsulate everything */
            ReturnValue += PrintEncapsulation(StringToEncapsulate, TabCount);

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
                            prerequisites = new KVList(FillFromKeyValueFolder<KVPair>(IterationKV.FolderValue));
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
            public ListeningList<String> PossibleDialogueInjections = new ListeningList<string>();
        }
    }

    public class QuestStage : QuestVariable
    {
        public string stageName;
        public string description;

        //public List<QuestInjectDialogue> affectedNPCs = new List<QuestInjectDialogue>();

        public List<QuestTask> tasks = new List<QuestTask>();
        public KVList dialogue = new KVList();
        public KVList particles = new KVList();

        public QuestStage_EditorExternal ThisEditorExternal = new QuestStage_EditorExternal();

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

        public static QuestStage KVGenerate(KVPair ThisKV, Quest Parent)
        {
            int Num = Convert.ToInt32(ThisKV.Key);
            QuestStage ReturnValue = Insert(Num, Parent);
            ReturnValue.GenerateFromKeyValue_Iterate(ThisKV.FolderValue, (KVPair IterationKV, FieldInfo ThisField) =>
            {
                switch (IterationKV.Key)
                {
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
                            });
                        }
                        break;
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
                                                obj2.ThisEditorExternal.EncapsulatingList = ReturnValue.dialogue;
                                            });
                                        }
                                        break;
                                    case "particle":
                                        {
                                            obj.FolderValue.Items.ForEach(obj2 =>
                                            {
                                                ReturnValue.particles.Add(GenerateFromKeyValue<KVPair>(obj2));
                                                obj2.ThisEditorExternal.EncapsulatingList = ReturnValue.particles;
                                            });
                                        }
                                        break;
                                }
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
                string TaskString = "";

                QuestIndexableVariableStringConverter<QuestTask_gather, QuestTask> GatherTaskList = new QuestIndexableVariableStringConverter<QuestTask_gather, QuestTask>("gather");
                QuestIndexableVariableStringConverter<QuestTask_location, QuestTask> LocationTasksList = new QuestIndexableVariableStringConverter<QuestTask_location, QuestTask>("location");
                QuestIndexableVariableStringConverter<QuestTask_talkto, QuestTask> TalkToTaskList = new QuestIndexableVariableStringConverter<QuestTask_talkto, QuestTask>("talkto");
                QuestIndexableVariableStringConverter<QuestTask_kill, QuestTask> KillTaskList = new QuestIndexableVariableStringConverter<QuestTask_kill, QuestTask>("kill");
                QuestIndexableVariableStringConverter<QuestTask_killType, QuestTask> KillTypeTaskList = new QuestIndexableVariableStringConverter<QuestTask_killType, QuestTask>("killType");

                tasks.ForEach(obj =>
                {
                    GatherTaskList.Add(obj);
                    LocationTasksList.Add(obj);
                    TalkToTaskList.Add(obj);
                    KillTaskList.Add(obj);
                    KillTypeTaskList.Add(obj);
                });

                TaskString += GatherTaskList.Print(TabCount + 1);
                TaskString += LocationTasksList.Print(TabCount + 1);
                TaskString += TalkToTaskList.Print(TabCount + 1);
                TaskString += KillTypeTaskList.Print(TabCount + 1);
                TaskString += KillTaskList.Print(TabCount + 1);

                stringtoencapsulate += PrintEncapsulation(TaskString, TabCount + 1, "tasks", true);
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

            bool HasConverted = false;
            if (ThisEditorExternal.AutoGenerateListenStringID && ThisOptionalFields.sendListenString != "") {
                ThisOptionalFields.sendListenString = Convert.ToString(ThisEditorExternal.Parent.questID) + "_" + ThisOptionalFields.sendListenString;
                HasConverted = true;
            }
            if (hideIf.Count > 0) {
                string hideIfString = "";

                QuestIndexableVariableStringConverter<QuestDialogueOptionHideIf_Quest, QuestDialogueOptionHideIf> QuestHideIfList = new QuestIndexableVariableStringConverter<QuestDialogueOptionHideIf_Quest, QuestDialogueOptionHideIf>("quests");

                hideIf.ForEach(obj => {
                    QuestHideIfList.Add(obj);
                });

                hideIfString += QuestHideIfList.Print(TabCount);

                ReturnValue += PrintEncapsulation(hideIfString, TabCount, "hideIf", true);
            }

            ReturnValue += ConvertToText_Iterate(TabCount);

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

            if (Response != null) {
                ReturnValue += PrintEncapsulation(Response.ConvertToText(TabCount + 1), TabCount, "goto", true);
            }

            if (HasConverted) {
                int NumberToRemove = ThisEditorExternal.Parent.questID.ToString().Length + 1;
                ThisOptionalFields.sendListenString.Remove(0, NumberToRemove);
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
                }
                /*if (obj.Key == "listeningQuest")
                {
                    ReturnValue.ThisEditorExternal.listeningQuest = Convert.ToInt32(obj.Value);
                }*/
            });
            if (ReturnValue.ThisOptionalFields.sendListenString != "") {
                int NumberToRemove = ReturnValue.ThisEditorExternal.Parent.questID.ToString().Length + 1;
                ReturnValue.ThisOptionalFields.ChangeListenString(ReturnValue.ThisOptionalFields.sendListenString.Remove(0, NumberToRemove));
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
                    Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings.Remove(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
                }
                this.sendListenString = sendListenString;
                if (this.sendListenString != "")
                {
                    Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings.Add(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
                }
            }

            public void ChangeID() {
                if (this.sendListenString != "")
                {
                    Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings.Remove(ThisEditorExternal.LastID + "_" + this.sendListenString);
                    Interpreter.CurrentQuestList.ThisEditorExternal.PossibleListenStrings.Add(ThisEditorExternal.Parent.questID + "_" + this.sendListenString);
                    ThisEditorExternal.LastID = ThisEditorExternal.Parent.questID;
                }
            }

            public class QuestDialogueOption_OptionalFields_EditorExternal : QuestVariableEditorExternal
            {
                public Quest Parent;
                public int LastID;
                /*public int giveQuest;
                public bool GivesQuest;*/
            }
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

            string OptionsString = "";
            int x = 0;

            options.ForEach(obj =>
            {
                x++;
                OptionsString += PrintEncapsulation(obj.ConvertToText(TabCount + 2), TabCount + 1, x.ToString(), true);
            });

            ReturnValue += PrintEncapsulation(OptionsString, TabCount, "options", true);

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

    public abstract class QuestDialogueOptionHideIf : MultiTypeVariable<QuestDialogueOptionHideIf>
    {
        public QuestDialogueOptionHideIf_EditorExternal ThisEditorExternal = new QuestDialogueOptionHideIf_EditorExternal();

        public override bool Trash()
        {
            ThisEditorExternal.ParentOption.hideIf.Remove(this);
            return true;
        }

        static QuestDialogueOptionHideIf() {
            AddPossibleTaskType("quests", typeof(QuestDialogueOptionHideIf_Quest));
        }

        public static void MassGenerate<T>(QuestDialogueOption Parent, KVPair obj, string typeString) where T : QuestDialogueOptionHideIf, new()
        {
            if (obj.Key == typeString)
            {
                obj.FolderValue.Items.ForEach(obj2 =>
                {
                    KVGenerate<T>(Parent, obj2);
                });
            }
        }

        public static T KVGenerate <T> (QuestDialogueOption Parent, KVPair ThisKV) where T : QuestDialogueOptionHideIf, new()
        {
            T ReturnValue = new T();
            GenerateEmpty(ReturnValue, Parent);
            ReturnValue.GenerateFromKV(ThisKV);
            return ReturnValue;
        }

        public static QuestDialogueOptionHideIf Generate(string TypeString, QuestDialogueOption Parent)
        {
            QuestDialogueOptionHideIf ReturnValue = GenerateEmpty(GetNewT(TypeString), Parent);
            ReturnValue.SetDefaultValues();
            return ReturnValue;
        }

        static QuestDialogueOptionHideIf GenerateEmpty(QuestDialogueOptionHideIf InputValue, QuestDialogueOption Parent)
        {
            QuestDialogueOptionHideIf ReturnValue = InputValue;
            ReturnValue.ThisEditorExternal.ParentOption = Parent;
            Parent.hideIf.Add(ReturnValue);
            return ReturnValue;
        }

        public virtual void SetDefaultValues() { }
    }

    public class QuestDialogueOptionHideIf_EditorExternal : QuestVariableEditorExternal {
        public QuestDialogueOption ParentOption;
    }

    public class QuestDialogueOptionHideIf_Quest : QuestDialogueOptionHideIf
    {
        public int Quest;
        public string QuestState;

        public static ListeningList<string> PossibleQuestStates = new ListeningList<string>();

        static QuestDialogueOptionHideIf_Quest()
        {
            PossibleQuestStates.Add("complete");
            PossibleQuestStates.Add("incomplete");
            PossibleQuestStates.Add("active");
        }

        public override string ConvertToText_Full(int Index, int TabCount = 0)
        {
            return PrintEncapsulation(PrintKeyValue(Quest.ToString(), QuestState, TabCount + 1), TabCount, Index.ToString(), true);
        }

        public override void GenerateFromKV(KVPair ThisKV)
        {
            ThisKV.FolderValue.Items.ForEach(obj =>
            {
                Quest = Convert.ToInt32(obj.Key);
                QuestState = obj.Value;
            });
        }

        public override void SetDefaultValues()
        {
            QuestState = "active";
        }
    }
}
