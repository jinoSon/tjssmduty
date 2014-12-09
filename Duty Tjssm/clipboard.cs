using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Duty_Tjssm
{
    class clipboard
    {
        Dictionary<String, TextBox > dictionaryMap;
        public clipboard()
        {
            dictionaryMap = new Dictionary<string, TextBox>();
        }

        public void insertTextBox(TextBox input, String where){
            dictionaryMap.Add(where, input);
        }
        public void insertString(String where){
            TextBox tm = new TextBox();
            tm.Text = where;
            insertTextBox(tm, where);
        }

        public void setClipboardText(){
            string result = mergeString();
            //클립보드에 result 삽입
            Clipboard.SetText(result);
        }


        public string mergeString()
        {
            string result = "";
            foreach(KeyValuePair<String, TextBox> t in  dictionaryMap){
                string temp = "";

                if (t.Key.Equals("2층"))
                {
                    temp += "\n";
                    temp += "[ 2층 ]";
                    temp += "\n";
                }
                else if (t.Key.Equals("3층"))
                {
                    temp += "\n";
                    temp += "[ 3층 ]";
                    temp += "\n";
                }
                else if (t.Key.Equals("복사기 사용량"))
                {
                    temp += "\n";
                    temp += "[ 복사기 사용량 ]";
                    temp += "\n";
                }
                else
                {
                    temp += "# " + t.Key+" : " + t.Value.Text + "\n";
                }
                result += temp;
            }
            result += "# 사용매수 : " + getUseA4() + "\n";

            return result;
        }
        public string getUseA4(){
            string result = "";
            TextBox baseTb = dictionaryMap["기준매수"];
            TextBox cuTb = dictionaryMap["현재매수"];
            int baseInt, cuInt;
            try
            {
                baseInt = Int32.Parse(baseTb.Text);
                cuInt = Int32.Parse(cuTb.Text);

            }
            catch
            {
                return "숫자가 아니네요";
            }
            
            return (baseInt-cuInt).ToString();

        }

       

    }
}
