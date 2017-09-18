using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ReIncarnation_Quest_Maker.Made_in_Abyss.QuestFormat;
using ReIncarnation_Quest_Maker.Obsidius;

namespace ReIncarnation_Quest_Maker.Made_in_Abyss.Parser
{
    public static class QuestFormatParser
    {

        public static void OpenTextFile(string FilePath) {
            try
            {
                string Raw = File.ReadAllText(FilePath);
                Parse(Raw, FilePath);
            }
            catch (IOException)
            {
            }
        }

        public static void Parse(string Raw, string FilePath) {
            string ErrorMessage;
            KVPairFolder All = ParseRaw(Raw, out ErrorMessage);
            if (ErrorMessage != null)
            {
                throw new ArgumentException(ErrorMessage);
            }
            Interpreter.LoadNewQuestList(QuestVariable.GenerateFromKeyValue<QuestList>(new KVPair("", All)), FilePath);
            All.Delete();
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

            //bool HasParsedKey = false;
            int LineCount = 0;
            int CharacterCount = 0;

            char[] Raw_CharArray = Raw.ToCharArray();
            for (int x = 0; x < Raw_CharArray.Length; x++)
            {
                bool ShouldParseIndiscrimately = ParseAsDefault; //true if last character was '\'

                if (!ShouldParseIndiscrimately)
                {
                    switch (Raw_CharArray[x])
                    {
                        case '\\':
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
                                if (Parsing) {
                                    /* Throws an Error Message */
                                    ErrorMessage = "Illegal character ({) (Character " + CharacterCount + " in line " + LineCount + ")";
                                    return null;
                                }
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
                            break;
                        case '}':
                            {
                                if (Parsing)
                                {

                                    /* Throws an Error Message */
                                    ErrorMessage = "Illegal character (}) (Character " + CharacterCount + " in line " + LineCount + ")";
                                }

                                CurrentFolder = CurrentFolder.Parent;

                                if (CurrentFolder == null) {
                                    /* Throws an Error Message */
                                    ErrorMessage = "Too many }. (Character " + CharacterCount + " in line " + LineCount + ")";
                                    return null;
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
                CharacterCount++;
                if (Raw_CharArray[x] == '\n')
                {
                    LineCount++;
                    CharacterCount = 0;
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
