using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.QuestFormat;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser
{

    public static class QuestFormatParser
    {
        public const char SafetyKey = '\\';
        public static string OpenTextFile(string FilePath) {
            try
            {
                return File.ReadAllText(FilePath);
                //Parse(Raw, FilePath);
            }
            catch (IOException)
            {
            }
            return null;
        }

        public static QuestList Parse(string Raw, string FilePath) {
            string ErrorMessage;
            KVPairFolder All = ParseRaw(Raw, out ErrorMessage);
            if (ErrorMessage != null)
            {
                throw new ArgumentException(ErrorMessage);
            }
            //Interpreter.LoadNewQuestList(QuestVariable.GenerateFromKeyValue<QuestList>(new KVPair("", All)), FilePath);
            QuestList ReturnValue = QuestVariable.GenerateFromKeyValue<QuestList>(GetQuestListFromAll(All));
            ReturnValue.ThisEditorExternal.FilePath = FilePath;
            All.Delete();
            return ReturnValue;
        }

        public static KVPair GetQuestListFromAll(KVPairFolder All) {
            return All.Items[0].FolderValue.Items[0];
        }

        public static KVPairFolder ParseRaw(string Raw, out string ErrorMessage) {

            /* Transforms text into a KeyValuePair folder */

            KVPairFolder ReturnValue = new KVPairFolder();

            KVPairFolder CurrentFolder = ReturnValue;
            bool Parsing = false;
            bool ParsingValue = false;
            string KeyValuePairKey = "";
            string KeyValuePairValue = "";
            bool ParseAsDefault = false;
            int ShouldNotParseLine = 1;

            //bool HasParsedKey = false;
            int LineCount = 0;
            int CharacterCount = 0;

            char[] Raw_CharArray = Raw.ToCharArray();
            for (int x = 0; x < Raw_CharArray.Length; x++)
            {
                if (ShouldNotParseLine != 0)
                {
                    //check if line became a comment
                    switch (Raw_CharArray[x]) {
                        case '/':
                            {
                                ShouldNotParseLine++;
                            }
                            break;
                        case ' ':
                        case '\t':
                            {
                                if (ShouldNotParseLine != 3) {
                                    ShouldNotParseLine = 1;
                                }
                            }
                            break;
                        default:
                            {
                                if (ShouldNotParseLine != 3) {
                                    ShouldNotParseLine = 0;
                                }
                            }
                            break;
                    }
                }
                if (ShouldNotParseLine == 0)
                {
                    bool ShouldParseIndiscrimately = ParseAsDefault; //true if last character was '\'

                    if (!ShouldParseIndiscrimately)
                    {
                        switch (Raw_CharArray[x])
                        {
                            case SafetyKey:
                                {
                                    ParseAsDefault = true;
                                }
                                break;
                            case '\"':
                                {
                                    if (Parsing)
                                    {
                                        if (ParsingValue)
                                        {
                                            //If finished parsing value, creates a new KeyValuePair.
                                            CurrentFolder.AddItem(new KVPair(KeyValuePairKey, KeyValuePairValue));
                                            KeyValuePairKey = "";
                                            KeyValuePairValue = "";
                                        }
                                        //Debug purposes
                                        //HasParsedKey = !ParsingValue;
                                        //When closing parsing, invert ParsingValue.
                                        ParsingValue = !ParsingValue;
                                    }
                                    //Inverts ifparsing
                                    Parsing = !Parsing;
                                }
                                break;
                            case '{':
                                {
                                    if (Parsing)
                                    {
                                        ShouldParseIndiscrimately = true;
                                    }
                                    else
                                    {

                                        KVPairFolder NewFolder = new KVPairFolder(CurrentFolder);
                                        CurrentFolder.AddItem(new KVPair(KeyValuePairKey, NewFolder));
                                        CurrentFolder = NewFolder;

                                        KeyValuePairKey = "";

                                        if (!ParsingValue)
                                        {
                                            /* Throws an Error Message */
                                            ErrorMessage = "Illegal character ({) (Character " + CharacterCount + " in line " + LineCount + ")";
                                            return null;
                                        }

                                        ParsingValue = false;
                                    }
                                }
                                break;
                            case '}':
                                {
                                    if (Parsing)
                                    {
                                        ShouldParseIndiscrimately = true;
                                    }
                                    else
                                    {
                                        CurrentFolder = CurrentFolder.Parent;

                                        if (CurrentFolder == null)
                                        {
                                            /* Throws an Error Message */
                                            ErrorMessage = "Too many }. (Character " + CharacterCount + " in line " + LineCount + ")";
                                            return null;
                                        }
                                    }
                                }
                                break;
                            default:
                                {
                                    if (Parsing)
                                    {
                                        ShouldParseIndiscrimately = true;
                                    }
                                }
                                break;
                        }
                    }
                    if (ShouldParseIndiscrimately)
                    {
                        if (ParsingValue)
                        {
                            KeyValuePairValue += Raw_CharArray[x];
                        }
                        else
                        {
                            KeyValuePairKey += Raw_CharArray[x];
                        }
                        ParseAsDefault = false;
                    }
                }
                CharacterCount++;
                if (Raw_CharArray[x] == '\n')
                {
                    LineCount++;
                    CharacterCount = 0;
                    ShouldNotParseLine = 1;
                }
            }

            if (CurrentFolder != ReturnValue) {
                /* Throws an Error Message */
                ErrorMessage = "Brackets are not closed.";
                return null;
            }

            if (Parsing)
            {
                /* Throws an Error Message */
                ErrorMessage = "Attempting to parse for no good reason. Close quotation marks.";
                return null;
            }

            if (ParsingValue)
            {
                /* Throws an Error Message */
                ErrorMessage = "Final key does not have value. Add a value to the last key.";
                return null;
            }

            ErrorMessage = null;
            return ReturnValue;
        }
    }

    public class KVPairFolder {
        public KVPairFolder Parent;
        public List<KVPair> Items = new List<KVPair> ();

        public KVPairFolder(KVPairFolder Parent = null) {
            this.Parent = Parent;
        }

        public void AddItem(KVPair Item)
        {
            Items.Add(Item);
        }

        public void Parse()
        {
            //Items.ForEach(obj => obj.Parse);
        }

        public void Delete() {
            Items.ForEach(obj => obj.Delete());
            Parent = null;
            Items.Clear();
        }
    }
}
