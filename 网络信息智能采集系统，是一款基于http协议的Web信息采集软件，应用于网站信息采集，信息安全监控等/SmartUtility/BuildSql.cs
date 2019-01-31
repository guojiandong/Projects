
namespace Smart.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;

	/// <summary>
	/// ����Sql���(����)
	/// </summary>
    public static class BuildSql
    {
        /// <summary>
        /// ͨ�������ͱ������ɲ������
        /// </summary>
        /// <param name="paraList">key=�ֶ�����value= �ֶ�ֵ���ֵ�</param>
        /// <param name="tableName">����</param>
        /// <returns>Insert ���</returns>
        public static StringBuilder BuildQueryString(Dictionary<string, string> paraList, string tableName)
        {
            StringBuilder insertString = new StringBuilder();
            insertString.Append("insert into ");
            insertString.Append(tableName);
            insertString.Append("( ");
            List<string> columnValue = new List<string>(paraList.Keys.Count);
            foreach (string key in paraList.Keys)
            {
                insertString.Append(key);
                insertString.Append(",");
                columnValue.Add(paraList[key]);
            }
            //ȥ��ĩβ��","
            insertString.Remove(insertString.Length - 1, 1);
            insertString.Append(" ) values( ");
            foreach (string value in columnValue)
            {
                if (value.StartsWith("to_date('"))
                {
                    //������������͵Ĳ���"'"
                    insertString.Append(value);
                    insertString.Append(",");
                }
                else
                {
                    insertString.Append("'" + value + "'");
                    insertString.Append(",");
                }
            }
            //ȥ��ĩβ��","
            insertString.Remove(insertString.Length - 1, 1);
            insertString.Append(" ) ");
            return insertString;
        }
    }
}
