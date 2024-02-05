using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsamblorARC
{
    internal class Program
    {
        static string sep = " ,:";
        static public List<string> instructions = new List<string>();
        static List<string> variables = new List<string>();
        static string[] variab = new string[10];
        static int line = 0;
        static int org = 0;
        static void ReadFile(string fn)
        {
            StreamReader load = new StreamReader(fn);

            string buffer;
            while ((buffer = load.ReadLine()) != null)
            {
                instructions.Add(buffer);
            }
        }
        static void Main(string[] args)
        {
            ReadFile(@"C:\Users\vanesa\source\repos\AsamblorARC\AsamblorARC\input.txt");
            FindVariables();
            line = org;
            for (int i = 0; i < instructions.Count; i++)
            {
                char[] result = new char[32];
                string[] lineInstr = instructions[i].Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!variables.Contains(lineInstr[0]) && !int.TryParse(lineInstr[0], out _))
                {
                    switch (lineInstr[0])
                    {
                        case "ld":

                        case "st":

                        case "sethi":

                        case "andcc":

                        case "orcc":

                        case "orncc":

                        case "srl":

                        case "addcc":

                        case "call":

                        case "jmpl":

                        case "be":

                        case "bneg":

                        case "bcs":

                        case "bvs":

                        case "ba":

                        case ".org":
                        case ".begin":
                        case ".end":
                            break;
                        default:
                            if (lineInstr.Length > 1) lineInstr = lineInstr.Skip(1).ToArray();
                            break;

                    }
                    ConvertInstruction(lineInstr, ref result);
                }
                else
                {
                    string nr;
                    int poz;
                    if (!int.TryParse(lineInstr[0], out _))
                        poz = 1;
                    else
                        poz = 0;
                    nr = ConvertToBase2(lineInstr[poz]);
                    if (int.Parse(lineInstr[poz]) > 0)
                        nr = nr.PadLeft(32, '0');
                    else
                        nr = nr.PadLeft(32, '1');
                    for (int j = 0; j < 32; j++)
                    {
                        result[31 - j] = nr[j];
                    }
                }
                if (lineInstr[0][0] != '.') Write(result);
                line += 4;
            }

            Console.ReadKey();
        }
        static void FindVariables()
        {
            bool startLineCount = false;
            for (int i = 0; i < instructions.Count; i++)
            {
                string[] lineInstr = instructions[i].Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (lineInstr[0] == ".org")
                { line = int.Parse(lineInstr[1]); org = line; }
                else
                {
                    if (!startLineCount)
                    {
                        if (lineInstr.Length > 1 && !(lineInstr[0][0] == '.' || lineInstr[1][0] == '.'))
                            startLineCount = true;
                    }
                    if (variables.Contains(lineInstr[0]))
                    {
                        variab[variables.IndexOf(lineInstr[0])] = line.ToString();
                    }
                    else
                        for (int j = 0; j < lineInstr.Length; j++)
                        {
                            if (lineInstr[j].Contains('[') && !int.TryParse(lineInstr[j].Trim('[', ']'), out _) && !variables.Contains(lineInstr[j].Trim('[', ']')))
                            {
                                variables.Add(lineInstr[j].Trim('[', ']'));
                            }
                        }
                    if (startLineCount)
                        line += 4;
                }
            }
        }
        private static void ConvertInstruction(string[] lineInstr, ref char[] result)
        {
            switch (lineInstr[0])
            {
                case "ld":
                    result[31] = '1';
                    result[30] = '1';

                    result[24] = '0';
                    result[23] = '0';
                    result[22] = '0';
                    result[21] = '0';
                    result[20] = '0';
                    result[19] = '0';

                    MemoryFormat(lineInstr, ref result);
                    break;
                case "st":
                    result[31] = '1';
                    result[30] = '1';

                    result[24] = '0';
                    result[23] = '0';
                    result[22] = '0';
                    result[21] = '1';
                    result[20] = '0';
                    result[19] = '0';

                    MemoryFormat(lineInstr, ref result);
                    break;
                case "sethi":
                    result[31] = '0';
                    result[30] = '0';

                    result[24] = '1';
                    result[23] = '0';
                    result[22] = '0';
                    break;
                case "andcc":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';
                    result[21] = '0';
                    result[20] = '0';
                    result[19] = '1';
                    ArithmeticFormat(lineInstr, ref result);
                    break;
                case "orcc":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';
                    result[21] = '0';
                    result[20] = '1';
                    result[19] = '0';
                    break;
                case "orncc":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';
                    result[21] = '1';
                    result[20] = '1';
                    result[19] = '0';
                    break;
                case "srl":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '1';
                    result[23] = '0';
                    result[22] = '0';
                    result[21] = '1';
                    result[20] = '1';
                    result[19] = '0';
                    break;
                case "addcc":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';
                    result[21] = '0';
                    result[20] = '0';
                    result[19] = '0';
                    ArithmeticFormat(lineInstr, ref result);
                    break;
                case "call":
                    result[31] = '0';
                    result[30] = '1';
                    break;
                case "jmpl":
                    result[31] = '1';
                    result[30] = '0';

                    result[24] = '1';
                    result[23] = '1';
                    result[22] = '1';
                    result[21] = '0';
                    result[20] = '0';
                    result[19] = '0';
                    result[13] = '1';
                    SetInResult(lineInstr, ref result, 1, 18, 5);
                    SetInResult(lineInstr, ref result, 3, 12, 13);
                    SetInResult(lineInstr, ref result, 4, 29, 5);
                    break;
                case "be":
                    result[31] = '0';
                    result[30] = '0';
                    result[29] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';

                    result[28] = '0';
                    result[27] = '0';
                    result[26] = '0';
                    result[25] = '1';
                    break;
                case "bneg":
                    result[31] = '0';
                    result[30] = '0';
                    result[29] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';

                    result[28] = '0';
                    result[27] = '1';
                    result[26] = '1';
                    result[25] = '0';
                    break;
                case "bcs":
                    result[31] = '0';
                    result[30] = '0';
                    result[29] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';

                    result[28] = '0';
                    result[27] = '1';
                    result[26] = '0';
                    result[25] = '1';
                    break;
                case "bvs":
                    result[31] = '0';
                    result[30] = '0';
                    result[29] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';

                    result[28] = '0';
                    result[27] = '1';
                    result[26] = '1';
                    result[25] = '1';
                    break;
                case "ba":
                    result[31] = '0';
                    result[30] = '0';
                    result[29] = '0';

                    result[24] = '0';
                    result[23] = '1';
                    result[22] = '0';

                    result[28] = '1';
                    result[27] = '0';
                    result[26] = '0';
                    result[25] = '0';
                    break;
                case ".org":
                    line = int.Parse(lineInstr[1]);
                    break;
                default:

                    break;
            }
        }
        private static void ArithmeticFormat(string[] instr, ref char[] result)
        {
            SetInResult(instr, ref result, 1, 18, 5);
            SetInResult(instr, ref result, 2, 12, 13);
            SetInResult(instr, ref result, 3, 29, 5);
        }
        private static void Write(char[] result)
        {
            Console.Write(result[31]);
            Console.Write(result[30]);
            Console.Write(' ');
            for (int i = 29; i >= 25; i--)
            {
                Console.Write(result[i]);
            }
            Console.Write(' ');
            for (int i = 24; i >= 19; i--)
            {
                Console.Write(result[i]);
            }
            Console.Write(' ');
            for (int i = 18; i >= 14; i--)
            {
                Console.Write(result[i]);
            }
            Console.Write(' ');
            Console.Write(result[13]);
            Console.Write(' ');
            for (int i = 12; i >= 0; i--)
            {
                Console.Write(result[i]);
            }

            Console.WriteLine();
        }
        private static string ConvertToBase2(string n)
        {
            string b2 = "";
            List<int> LI = new List<int>();
            int intreg = int.Parse(n);
            if (intreg > 0)
            {
                if (intreg == 0) LI.Add(0);
                while (intreg != 0)
                {
                    LI.Add(intreg % 2);
                    intreg /= 2;
                }
                for (int i = LI.Count - 1; i >= 0; i--)
                {
                    b2 += LI[i];
                }
            }
            else
            {
                intreg *= -1;
                intreg--;
                while (intreg != 0)
                {
                    LI.Add(intreg % 2);
                    intreg /= 2;
                }
                for (int i = LI.Count - 1; i >= 0; i--)
                {
                    if (LI[i] == 0) b2 += 1;
                    else
                        b2 += 0;
                }

            }
            return b2;
        }
        private static void MemoryFormat(string[] instr, ref char[] result)
        {
            for (int i = 0; i < 6; i++)
            {
                result[18 - i] = '0';
            }
            if (instr[0] == "ld")
            {
                SetInResult(instr, ref result, 1, 12, 13);
                SetInResult(instr, ref result, 2, 29, 5);
            }
            else
            {
                SetInResult(instr, ref result, 2, 12, 13);
                SetInResult(instr, ref result, 1, 29, 5);
            }
        }
        private static void SetInResult(string[] instr, ref char[] result, int pozinput, int pozResult, int lenght)
        {
            if (!(int.TryParse(instr[pozinput], out _)))
            {
                if (instr[pozinput][1] == 'r')
                {
                    if (result[13] != '1') result[13] = '0';
                    if (instr[pozinput].Length == 4)
                    {
                        string nr = "";
                        nr += instr[pozinput][2];
                        nr += instr[pozinput][3];
                        nr = ConvertToBase2(nr);
                        string ahh = nr.PadLeft(lenght, '0');
                        nr = ahh;
                        for (int i = 0; i < lenght; i++)
                        {
                            result[pozResult - i] = nr[i];
                        }
                    }
                    else
                    {
                        string nr = "";
                        nr += instr[pozinput][2];
                        nr = ConvertToBase2(nr);
                        string ahh = nr.PadLeft(lenght, '0');
                        nr = ahh;
                        for (int i = 0; i < lenght; i++)
                        {
                            result[pozResult - i] = nr[i];
                        }
                    }
                }
                else
                {

                    result[13] = '1';
                    string nr = instr[pozinput];
                    string ahh = nr.Trim('[', ']');
                    nr = ahh;

                    if (variables.Contains(nr))
                    {
                        string position = variab[variables.IndexOf(nr)];
                        nr = ConvertToBase2(position.ToString());
                        ahh = nr.PadLeft(lenght, '0');
                        nr = ahh;
                        for (int i = 0; i < lenght; i++)
                        {
                            result[pozResult - i] = nr[i];
                        }

                    }


                }
            }
            else
            {

                string nr = instr[pozinput];
                nr = ConvertToBase2(nr);
                string ahh;
                if (int.Parse(instr[pozinput]) > 0)
                    ahh = nr.PadLeft(lenght, '0');
                else ahh = nr.PadLeft(lenght, '1');
                nr = ahh;
                for (int i = 0; i < lenght; i++)
                {
                    result[pozResult - i] = nr[i];
                }
            }
        }

    }
}