using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using NPOI.HPSF;
using NPOI.XSSF.UserModel;
using Model;
using System.Globalization;
using System.Data;
using System.Web.UI;

namespace Common
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHelper
    {
        #region Excel导入

        /// <summary>
        /// 从Excel取数据并记录到List集合里
        /// </summary>
        /// <param name="filePath">保存文件绝对路径</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>转换后的List对象集合</returns>
        public static void ExcelToEntityList(string filePath, ref StringBuilder errorMsg)
        {
            //try
            //{
                if (Regex.IsMatch(filePath, ".xls$")) // 2003
                {
                    Excel2003ToEntityList(filePath, ref errorMsg);
                }
                else if (Regex.IsMatch(filePath, ".xlsx$")) // 2007
                {
                    Excel2007ToEntityList(filePath, ref errorMsg);
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        //W1制作部良明细 解析函数
        private static void Excel2007ToEntityList(string filePath, ref StringBuilder errorMsg)
        {
            List<QualityEntity> QualityEntityList = new List<QualityEntity>();
            List<QualityEntity> QualityEntityList1 = new List<QualityEntity>();
            List<string> DateList = new List<string>();
            List<string> LineList = new List<string>();
            dateTimeDic.Clear();
            using (FileStream fs = File.OpenRead(filePath))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(2); // 获取此文件第一个Sheet页
                IRow row;
                ICell cell;
                string date = string.Empty;
                string line = string.Empty;
                string section = string.Empty;
                string problemName = string.Empty;
                QualityEntity entity;
                List<string> rowDataList;
                if (sheet.LastRowNum <= 5)
                    return;

                for (int _rowNum = 4; _rowNum <= sheet.LastRowNum - 1; _rowNum++) // 从1开始，第0行为单元头
                {
                    #region 日期+线别
                    // 1.判断当前行是否空行，若空行就不在进行读取下一行操作，结束Excel读取操作
                    row = sheet.GetRow(_rowNum);
                    if (row == null)
                        break;
                    if(_rowNum == 4)    //日期 date
                    {
                        rowDataList = GetCells(row);
                        if (rowDataList != null && rowDataList.Count > 0)
                        {
                            DateList = rowDataList;
                        }
                        continue;
                    }
                    else if(_rowNum == 5) // 线别Line CA013 
                    {
                        rowDataList = GetCells(row);
                        if (rowDataList != null && rowDataList.Count > 0)
                        {
                            LineList = rowDataList;
                        }
                        continue;
                    }
                    #endregion

                    #region 真实数据
                    //if (_rowNum < 8) //暂时略过Input & Output
                    //    continue;

                    problemName = row.GetCell(4).ToString();
                    if(problemName.Contains("Input"))
                    {
                        if (row.GetCell(3) == null && row.GetCell(3).ToString() == null)
                        {
                            section = "M1";
                        }
                        else
                        {
                            section = row.GetCell(3).ToString();
                        }
                        if (section == null || section == "")
                        {
                            section = "M1";
                        }
                    }
                    else if(problemName.Contains("Output"))
                    {
                        if (row.GetCell(3) == null && row.GetCell(3).ToString() == null)
                        {
                            section = "TP";
                        }
                        else
                        {
                            section = row.GetCell(3).ToString();
                        }

                        if (section == null || section == "")
                        {
                            section = "TP";
                        }
                    }
                    else
                        section = row.GetCell(3).ToString();

                    for (int _cellNum = 5;_cellNum < DateList.Count - 6; _cellNum++)
                    {
                        cell = row.GetCell(_cellNum);
                        if (cell!= null && cell.CellType != CellType.Blank && !string.IsNullOrEmpty( cell.ToString().Trim() ))
                        {
                            try
                            {
                                date = DateList[_cellNum - 1]; //date
                                line = LineList[_cellNum - 1]; //lineid
                                
                                string cellValue = cell.ToString();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    entity = new QualityEntity();
                                    entity.Section = section.ToLower();
                                    entity.ProblemName = problemName;
                                    entity.UserLineCode = line;
                                    if (IsNumeric(cellValue))
                                        entity.Value = int.Parse(cellValue);
                                    else
                                    {
                                        try
                                        {
                                            if (cellValue.Contains("+"))
                                            {
                                                cellValue = cellValue.Replace("=", "");
                                                string[] arr = cellValue.Split('+');
                                                int realValue = 0;
                                                for (int k = 0; k < arr.Length; k++)
                                                {
                                                    realValue += int.Parse(arr[k]);
                                                }
                                                entity.Value = realValue;
                                            }
                                            else
                                            {
                                                entity.Value = 0;
                                            }
                                        }
                                        catch
                                        {
                                            entity.Value = 0;
                                        }
                                    }
                                    ParseDateTime(date, ref entity);
                                    QualityEntityList.Add(entity);
                                    if (date.Contains("30-11月-2018"))
                                    {
                                        Console.WriteLine("date : " + date + "  lien : " + line);
                                        QualityEntityList1.Add(entity);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = " CheckExcel2007 section = " + section + "  problemName: " + problemName + " Columm = " + _cellNum
                                    + " Value :" + cell.ToString() + "  Date :" + date + "  UserLineCode : " + line +
                                    "  Exception Msg :" + ex.Message;
                                errorMsg.AppendLine(msg);
                                //return;
                            }
                        }
                    }
                    #endregion
                }
            }

            if (errorMsg.Length != 0)
            {
                return;
            }
           // check(ref QualityEntityList);
            InitSql();
            DataIntegrate(ref QualityEntityList, ref errorMsg);
            InsertOperator(ref QualityEntityList);
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static void check(ref List<QualityEntity> qualityEntityList)
        {
            int count = 0;
            List<QualityEntity> list = new List<QualityEntity>();
            List<int> list_1 = new List<int>();
            foreach (QualityEntity entity in qualityEntityList)
            {
                if (entity.Date == "2020-7-31")
                {
                    list.Add(entity);
                    list_1.Add(entity.Value);
                    count += 1;
                }
            }
        }

        //初始化Sql-Table数据
        private static void InitSql()
        {
            SqlHelper.InitData();
        }

        // 数据整合
        private static void DataIntegrate(ref List<QualityEntity> qualityEntityList, ref StringBuilder errorMsg)
        {
            List<int> _list = new List<int>();
           // StringBuilder builder1 = new StringBuilder();
            foreach (QualityEntity entity in qualityEntityList)
            {
                
                string processId = string.Empty;
              
                if (SqlHelper.dic_tb_process_id_code.TryGetValue(entity.Section.Replace(" ", "").ToLower(), out processId))
                {
                    entity.ProcessId = int.Parse(processId);
                }
                else
                {
                  //  builder1.AppendLine("entity.Section :" + entity.Section);
                    errorMsg.AppendLine("entity.Section :" + entity.Section);
                }

                string lineId = string.Empty;
                if (SqlHelper.dic_tb_userline.TryGetValue(entity.UserLineCode.Replace(" ", "").ToLower(), out lineId))
                {
                    entity.LineId = int.Parse(lineId);
                }
                else
                {
                    //builder1.AppendLine("entity.UserLineCode :" + entity.UserLineCode);
                    errorMsg.AppendLine("entity.UserLineCode :" + entity.UserLineCode);
                }

                string subjectId = string.Empty;
                if (entity.ProblemName.Contains("间隙"))
                {
                    string aa = entity.Section.Replace(" ", "").ToLower();
                }
                if (SqlHelper.dic_tb_subject.TryGetValue(entity.ProblemName.Replace(" ", "").ToLower() + "_"+ entity.ProcessId, out subjectId))
                {
                    entity.SubjectId = int.Parse(subjectId);
                }
                else
                {
                   // builder1.AppendLine("entity.ProblemName :" + entity.ProblemName);
                    errorMsg.AppendLine("entity.ProblemName :" + entity.ProblemName);
                }


                string theBest = string.Empty;
                if (SqlHelper.dic_tb_process_best.TryGetValue(entity.ProcessId.ToString(), out theBest))
                {
                    entity.TheBest = int.Parse(theBest);
                }

                string theNorm = string.Empty;
                if (SqlHelper.dic_tb_process_norm.TryGetValue(entity.ProcessId.ToString(), out theNorm))
                {
                    entity.TheNorm = int.Parse(theNorm);
                }

                string best = string.Empty;
                if (SqlHelper.dic_tb_subject_best.TryGetValue(entity.ProblemName.Replace(" ", "").ToLower() + "_" + entity.ProcessId, out best))
                {
                    if (string.IsNullOrEmpty(best))
                    {
                        entity.Best = entity.Value;
                        return;
                    }
                    int best_int = int.Parse(best);
                    if (best_int == -1)
                        entity.Best = entity.Value;
                    else
                        entity.Best = Math.Min(entity.Value, best_int);
                }

                string norm = string.Empty;
                if (SqlHelper.dic_tb_subject_norm.TryGetValue(entity.ProblemName.Replace(" ", "").ToLower() + "_" + entity.ProcessId, out norm))
                {
                    if (string.IsNullOrEmpty(norm))
                    {
                        entity.Norm = entity.Value;
                        return;
                    }
                    int norm_int = int.Parse(norm);
                    if (norm_int == -1)
                        entity.Norm = entity.Value;
                    else
                        entity.Norm = Math.Min(entity.Value, norm_int);
                }

                string classId = string.Empty;
                if (SqlHelper.dic_tb_line_workday.TryGetValue(entity.UserLineCode.Replace(" ", "").ToLower() + "_" + entity.Month, out classId))
                {
                    entity.ClassId = int.Parse(classId);
                }
                else
                {
                    // builder1.AppendLine("entity.ProblemName :" + entity.ProblemName);
                    errorMsg.AppendLine("entity.UserLineCode :" + entity.UserLineCode + " entity.Month :" + entity.Month);
                }

                //int result = entity.UserLineCode.CompareTo("CA010");
                //if (result <= 0)
                //{
                //    int index = entity.Month % 13 - 1;
                //    entity.ClassId = ConstVar.NightShift[index];
                //}
                //else if (entity.UserLineCode.CompareTo("CA020") <= 0)
                //{
                //    int index = entity.Month % 13 - 1;
                //    entity.ClassId = ConstVar.DayShift[index];
                //}
                //else if (entity.UserLineCode.CompareTo("CA021") == 0)
                //{
                //    int index = entity.Month % 13 - 1;
                //    entity.ClassId = ConstVar.DayShift_21[index];
                //}
                //else if (entity.UserLineCode.CompareTo("CA024") == 0)
                //{
                //    int index = entity.Month % 13 - 1;
                //    entity.ClassId = ConstVar.DayShift_24[index];
                //}
                //_list.Add(entity.Value);
            }
        }

        //private static void InsertOperator(ref List<QualityEntity> qualityEntityList)
        //{
        //    Dictionary<string, List<QualityEntity>> dic_Quallity = new Dictionary<string, List<QualityEntity>>();
        //    foreach (QualityEntity _entity in qualityEntityList)
        //    {
        //        List<QualityEntity> _qualityEntities = null;
        //        if (dic_Quallity.TryGetValue(_entity.Year.ToString(), out _qualityEntities))
        //        {
        //            _qualityEntities.Add(_entity);
        //        }
        //        else
        //        {
        //            dic_Quallity.Add(_entity.Year.ToString(), new List<QualityEntity>() { _entity });
        //        }
        //    }
        //    CreateSql(ref dic_Quallity);
        //}

        private static void CreateSql(ref Dictionary<string, List<QualityEntity>> dic_Quallity)
        {
            foreach (KeyValuePair<string, List<QualityEntity>> kvp in dic_Quallity)
            {
                #region sql 拼接 builder
                List<QualityEntity> qualityEntityList = kvp.Value;
                StringBuilder builder = new StringBuilder();
                builder.Append(" create table if not exists ");
                builder.Append("tb_quality_"+kvp.Key);
                builder.Append("(`id` int PRIMARY KEY NOT NULL auto_increment,`lineId` int NOT NULL,`subjectId` int NOT NULL,`classId` int NOT NULL,`date`  date NOT NULL,`value` int NOT NULL,`userLineCode` varchar(20) NOT NULL,`best` int NOT NULL,`norm` int NOT NULL,`theBest` int NOT NULL,`theNorm` int NOT NULL,UNIQUE INDEX Index9(lineId, subjectId, classId, date),INDEX userLineCode(userLineCode))ENGINE = InnoDB DEFAULT CHARSET = utf8;");
                builder.Append("insert into ");
                builder.Append("tb_quality_" + kvp.Key + "(" +
                "lineId," +
                "subjectId," +
                "classId," +
                "date," +
                "userLineCode," +
                "value," +
                "best," +
                "norm," +
                "theBest," +
                "theNorm ) values ");

                for (int i = 0; i < qualityEntityList.Count; i++)
            {
                QualityEntity entity = qualityEntityList[i];
                builder.Append("(");
                builder.Append("'" + entity.LineId + "',");
                builder.Append("'" + entity.SubjectId + "',");
                builder.Append("'" + entity.ClassId + "',");
                builder.Append("'" + entity.Date + "',");
                builder.Append("'" + entity.UserLineCode + "',");
                builder.Append("'" + entity.Value + "',");
                builder.Append("'" + entity.Best + "',");
                builder.Append("'" + entity.Norm + "',");
                builder.Append("'" + entity.TheBest + "',");
                builder.Append("'" + entity.TheNorm + "'");
                if (i < qualityEntityList.Count - 1)
                    builder.Append("),");
                else
                    builder.Append(") on duplicate key update value=VALUES(value);");
            }
                string realSql = builder.ToString();
                SqlHelper.ExecuteInsert(realSql);
                #endregion
            }
        }

        //数据库插入操作
        private static void InsertOperator(ref List<QualityEntity> qualityEntityList)
        {
            #region sql 拼接 builder
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into tb_quality_2018( " +
                "lineId," +
                "subjectId," +
                "classId," +
                "date," +
                //"year," +
                //"month," +
                //"day," +
                //"week," +
                "userLineCode," +
                "value," +
                "best," +
                "norm," +
                "theBest," +
                "theNorm ) values ");

            for (int i = 0; i < qualityEntityList.Count; i++)
            {
                QualityEntity entity = qualityEntityList[i];
                builder.Append("(");
                builder.Append("'" + entity.LineId + "',");
                builder.Append("'" + entity.SubjectId + "',");
                builder.Append("'" + entity.ClassId + "',");
                builder.Append("'" + entity.Date + "',");
                //builder.Append("'" + entity.Year + "',");
                //builder.Append("'" + entity.Month + "',");
                //builder.Append("'" + entity.Day + "',");
                //builder.Append("'" + entity.Week + "',");
                builder.Append("'" + entity.UserLineCode + "',");
                builder.Append("'" + entity.Value + "',");
                builder.Append("'" + entity.Best + "',");
                builder.Append("'" + entity.Norm + "',");
                builder.Append("'" + entity.TheBest + "',");
                builder.Append("'" + entity.TheNorm + "'");
                if (i < qualityEntityList.Count - 1)
                    builder.Append("),");
                else
                    builder.Append(") on duplicate key update value=VALUES(value) ;");
            }

            string realSql = builder.ToString();
            SqlHelper.ExecuteInsert(realSql);
            #endregion

        }

        public class DateTimeFormat
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public int Week { get; set; }
            public string Date { get; set; }
            public DateTimeFormat(string date, int year, int month, int day, int week)
            {
                Date = date;
                Year = year;
                Month = month;
                Day = day;
                Week = week;
            }
        }


        private static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        // 存储 时间类
        private static Dictionary<string, DateTimeFormat> dateTimeDic = new Dictionary<string, DateTimeFormat>();
        /// <summary>
        //时间解析函数 
        /// </summary>
        /// <param name="date_str">2018-6月-21</param>
        /// <param name="entity">要复制的类实例</param>
        private static void ParseDateTime(string date_str, ref QualityEntity entity)
        {
            if (string.IsNullOrEmpty(date_str) || entity == null)
                return;

            string date = Regex.Replace(date_str, @"[\u4e00-\u9fa5]", ""); //去除汉字
            DateTimeFormat format;
            if (dateTimeDic.TryGetValue(date, out format))
            {
                entity.Date = format.Date;
                entity.Year = format.Year;
                entity.Month = format.Month;
                entity.Day = format.Day;
                entity.Week = format.Week;
            }
            else
            {
                string[] arr = date.Split('-');
                int year = int.Parse(arr[2]);
                int month = int.Parse(arr[1]);
                int day = int.Parse(arr[0]);
                DateTime dt = new DateTime(year, month, day);
                int week = GetWeekOfYear(dt);

                string date_format = string.Format("{0:d}", dt).Replace('/', '-');
                entity.Date = date_format;
                entity.Year = year;
                entity.Month = month;
                entity.Day = day;
                entity.Week = week;
                dateTimeDic.Add(date, new DateTimeFormat(date_format, year, month, day, week));
            }

        }

        private static List<string> GetCells(IRow row)
        {
            ICell cell;
            List<string> rowDataList = new List<string>();
            if (row == null)
                return null;
            for(int i = 0;i<row.LastCellNum;i++)
            {
                cell = row.GetCell(i);
                if (cell != null)
                {
                    rowDataList.Add(cell.ToString());
                }
            }
            if(rowDataList.Count > 0)
                rowDataList.RemoveAt(0);
            return rowDataList;
        }


        private static void Excel2003ToEntityList(string filePath, ref StringBuilder errorMsg)
        {
            List<QualityEntity> QualityEntityList = new List<QualityEntity>();
            List<string> DateList = new List<string>();
            List<string> LineList = new List<string>();
            dateTimeDic.Clear();
            using (FileStream fs = File.OpenRead(filePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0); // 获取此文件第一个Sheet页
                IRow row;
                ICell cell;
                string date = string.Empty;
                string line = string.Empty;
                string section = string.Empty;
                string problemName = string.Empty;
                QualityEntity entity;
                List<string> rowDataList;
                if (sheet.LastRowNum <= 5)
                    return;

                for (int _rowNum = 4; _rowNum <= sheet.LastRowNum - 1; _rowNum++) // 从1开始，第0行为单元头
                {
                    #region 日期+线别
                    // 1.判断当前行是否空行，若空行就不在进行读取下一行操作，结束Excel读取操作
                    row = sheet.GetRow(_rowNum);
                    if (row == null)
                        break;
                    if (_rowNum == 4)    //日期 date
                    {
                        rowDataList = GetCells(row);
                        if (rowDataList != null && rowDataList.Count > 0)
                        {
                            DateList = rowDataList;
                        }
                        continue;
                    }
                    else if (_rowNum == 5) // 线别Line CA013 
                    {
                        rowDataList = GetCells(row);
                        if (rowDataList != null && rowDataList.Count > 0)
                        {
                            LineList = rowDataList;
                        }
                        continue;
                    }
                    #endregion

                    #region 真实数据
                    //if (_rowNum < 8) //暂时略过Input & Output
                    //    continue;

                    problemName = row.GetCell(4).ToString();
                    if (problemName.Contains("Input"))
                        section = "Offfline-1";
                    else if (problemName.Contains("Output"))
                        section = "TP";
                    else
                        section = row.GetCell(3).ToString();

                    for (int _cellNum = 5; _cellNum < DateList.Count - 6; _cellNum++)
                    {
                        cell = row.GetCell(_cellNum);
                        if (cell != null && cell.CellType != CellType.Blank && !string.IsNullOrEmpty(cell.ToString().Trim()))
                        {
                            try
                            {
                                date = DateList[_cellNum - 1]; //date
                                line = LineList[_cellNum - 1]; //lineid
                                Console.WriteLine("date : " + date + "  lien : " + line);
                                string cellValue = cell.ToString();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    entity = new QualityEntity();
                                    entity.Section = section.ToLower();
                                    entity.ProblemName = problemName;
                                    entity.UserLineCode = line;
                                    entity.Value = int.Parse(cellValue);
                                    ParseDateTime(date, ref entity);
                                    QualityEntityList.Add(entity);
                                }

                            }
                            catch (Exception ex)
                            {
                                string msg = " CheckExcel2007 section = " + section + "  problemName: " + problemName + " Columm = " + _cellNum
                                    + " Value :" + cell.ToString() + "  Date :" + date + "  UserLineCode : " + line +
                                    "  Exception Msg :" + ex.Message;
                                errorMsg.AppendLine(msg);
                                //return;
                            }
                        }
                    }
                    #endregion
                }
            }

            if (errorMsg.Length != 0)
            {
                return;
            }
            InitSql();
            DataIntegrate(ref QualityEntityList, ref errorMsg);
            InsertOperator(ref QualityEntityList);
        }

        #endregion Excel导入

        #region Excel导出

        /// <summary>
        /// 实体类集合导出到EXCLE2003
        /// </summary>
        /// <param name="cellHeard">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>文件的下载地址</returns>
        public static string EntityListToExcel2003(Dictionary<string, string> cellHeard, IList enList, string sheetName)
        {
            try
            {
                string fileName = sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
                string urlPath = "UpFiles/ExcelFiles/" + fileName; // 文件下载的URL地址，供给前台下载
                string filePath = HttpContext.Current.Server.MapPath("\\" + urlPath); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 2.解析单元格头部，设置单元头的中文名称
                HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
                IRow row = sheet.CreateRow(0);
                List<string> keys = cellHeard.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(cellHeard[keys[i]]); // 列名为Key的值
                }

                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow rowTmp = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        System.Reflection.PropertyInfo properotyInfo = null; // 属性的信息

                        // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                        if (keys[i].IndexOf(".") >= 0)
                        {
                            // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                            string[] properotyArray = keys[i].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                            string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                            System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                            if (subClassInfo != null)
                            {
                                // 3.1.2 获取子类的实例
                                var subClassEn = en.GetType().GetProperty(subClassName).GetValue(en, null);
                                // 3.1.3 根据属性名称获取子类里的属性类型
                                properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                                if (properotyInfo != null)
                                {
                                    properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                                }
                            }
                        }
                        else
                        {
                            // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                            properotyInfo = en.GetType().GetProperty(keys[i]);
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(en, null);
                            }
                        }

                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }

                        // 3.4 填充到Excel的单元格里
                        rowTmp.CreateCell(i).SetCellValue(cellValue);
                    }
                    rowIndex++;
                }

                // 4.生成文件
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();

                // 5.返回下载路径
                return urlPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Excel导出

        /// <summary>
        /// 保存Excel文件
        /// <para>Excel的导入导出都会在服务器生成一个文件</para>
        /// <para>路径：UpFiles/ExcelFiles</para>
        /// </summary>
        /// <param name="file">传入的文件对象</param>
        /// <returns>如果保存成功则返回文件的位置;如果保存失败则返回空</returns>
        public static string SaveExcelFile(HttpPostedFile file)
        {
            try
            {
                var fileName = file.FileName.Insert(file.FileName.LastIndexOf('.'), "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UpFiles/ExcelFiles"), fileName);
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                file.SaveAs(filePath);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }


        public static void ExportReminder(DataTable dataTable, ref string result)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return;
            }
            List<string> base_header_list = new List<string>() { "ProjectName", "Deadline", "Remark", "CurDate", "Progress","Name"};
            int rowIndex = 0;
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook();
                ISheet sheet;
                sheet = workbook.CreateSheet("Reminder");
                IRow row_base_header = sheet.CreateRow(rowIndex);
                for (int column = 0; column < base_header_list.Count; column++)
                    row_base_header.CreateCell(column).SetCellValue(base_header_list[column]);
                rowIndex++;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    IRow row_base = sheet.CreateRow(rowIndex);
                    for (int column1 = 0; column1 < base_header_list.Count; column1++)
                    {
                        if (dataRow[column1] != null)
                            row_base.CreateCell(column1).SetCellValue(dataRow[column1].ToString());
                        else
                            row_base.CreateCell(column1).SetCellValue(" ");
                    }
                    rowIndex++;
                }

                //2.创建流对象并设置存储Excel文件的路径
                using (FileStream url = File.OpenWrite(@"D:\Reminder.xlsx"))
                {
                    workbook.Write(url);
                };
                workbook.Close();
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
        }
    }
}