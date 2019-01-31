
namespace Smart.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;

	/// <summary>
	/// ���������ڽ�Сд����ת��Ϊ
	/// 1. һ�����Ĵ�д����
	/// 2. ����Ҵ�д����
	/// </summary>
	public class NumberToChineseChar
	{
		#region ˽�б���
		/// <summary>
		/// һ���д������������
		/// </summary>
		private static char[] chnGenText;
		/// <summary>
		/// һ���д����
		/// </summary>
		private static char[] chnGenDigit;

		/// <summary>
		/// ���������
		/// </summary>
		private static char[] chnRMBText;
		/// <summary>
		/// ����Ҽ�����λ������
		/// </summary>
		private static char[] chnRMBDigit;
		/// <summary>
		/// ����ҵ�λ���Ƿ֣�
		/// </summary>
		private static char[] chnRMBUnit;
		#endregion

		#region ���캯��
		/// <summary>
		/// ���캯��
		/// </summary>
		static NumberToChineseChar()
		{
			// һ���д����������
			chnGenText = new char[] { '��', 'һ', '��', '��', '��', '��', '��', '��', '��', '��' };
			chnGenDigit = new char[] { 'ʮ', '��', 'ǧ', '��', '��' };

			// �����ר��������
			chnRMBText = new char[] { '��', 'Ҽ', '��', '��', '��', '��', '½', 'Ⱦ', '��', '��' };
			chnRMBDigit = new char[] { 'ʰ', '��', 'Ǫ', '�f', '�|' };
			chnRMBUnit = new char[] { '��', '��' };
		}
		#endregion

		#region ��ת������
		/// <summary>
		/// ��ת������
		/// </summary>
		/// <param name="numberChar">��ת�������ַ���</param>
		/// <param name="convertToRMB">�Ƿ�ת���������</param>
		/// <returns>ת���ɵĴ�д�ַ���</returns>
		public static string Convert(string numberChar, bool convertToRMB)
		{
			// �������������Ч��
			CheckDigit(ref numberChar, convertToRMB);

			// �������ַ���
			StringBuilder chineseText = new StringBuilder();

			// ��ȡ���Ų���
			ExtractSign(ref chineseText, ref numberChar, convertToRMB);

			// ��ȡ��ת��������С������
			ConvertNumber(ref chineseText, ref numberChar, convertToRMB);

			return chineseText.ToString();
		}

		/// <summary>
		/// ��ת������
		/// </summary>
		/// <param name="number">��ת������</param>
		/// <param name="convertToRMB">�Ƿ�ת���������</param>
		/// <returns>ת���ɵĴ�д�ַ���</returns>
		public static string Convert(int number, bool convertToRMB)
		{
			return Convert(number.ToString(), convertToRMB);
		}

		/// <summary>
		/// ��ת������
		/// </summary>
		/// <param name="number">��ת������</param>
		/// <param name="convertToRMB">�Ƿ�ת���������</param>
		/// <returns>ת���ɵĴ�д�ַ���</returns>
		public static string Convert(decimal number, bool convertToRMB)
		{
			return Convert(number.ToString(), convertToRMB);
		}

		/// <summary>
		/// ��ת������
		/// </summary>
		/// <param name="number">��ת������</param>
		/// <returns>ת���ɵĴ�д�ַ���</returns>
		public static string Convert(int number)
		{
			return Convert(number, false);
		}
		#endregion

		#region ˽�з���
		/// <summary>
		/// ת������
		/// </summary>
		/// <param name="chineseText">ת������ַ���</param>
		/// <param name="numberChar">�����ַ���</param>
		/// <param name="convertToRMB">�Ƿ�ת��Ϊ����� true:ת�� false:��ת��</param>
		protected static void ConvertNumber(ref StringBuilder chineseText, ref string numberChar, bool convertToRMB)
		{
			int indexOfPoint;
			if (-1 == (indexOfPoint = numberChar.IndexOf('.'))) // ���û��С������
			{
				chineseText.Append(ConvertIntegral(numberChar, convertToRMB));

				if (convertToRMB)  // ���ת���������
				{
					chineseText.Append("Բ��");
				}
			}
			else // ��С������
			{
				// ��ת����������
				if (0 == indexOfPoint) // �����.���ǵ�һ���ַ�
				{
					if (!convertToRMB)  // ���ת����һ�����Ĵ�д
					{
						chineseText.Append('��');
					}
				}
				else // �����.�����ǵ�һ���ַ�
				{
					chineseText.Append(ConvertIntegral(numberChar.Substring(0, indexOfPoint), convertToRMB));
				}

				// ��ת��С������
				if (numberChar.Length - 1 != indexOfPoint) // �����.���������һ���ַ�
				{
					if (convertToRMB) // ���ת���������
					{
						if (0 != indexOfPoint) // �����.�����ǵ�һ���ַ�
						{
							if (1 == chineseText.Length && "��" == chineseText.ToString()) // �����������ֻ��'0'
							{
								chineseText.Remove(0, 1);  // ȥ�����㡱
							}
							else
							{
								chineseText.Append('Բ');
							}
						}
					}
					else
					{
						chineseText.Append('��');
					}

					string strTmp = ConvertFractional(numberChar.Substring(indexOfPoint + 1), convertToRMB);

					if (0 != strTmp.Length) // С�������з���ֵ
					{
						if (convertToRMB && // ���ת��Ϊ�����
						 0 == chineseText.Length && // ��û����������
						 "��" == strTmp.Substring(0, 1)) // �ҷ����ִ��ĵ�һ���ַ��ǡ��㡱
						{
							chineseText.Append(strTmp.Substring(1));
						}
						else
						{
							chineseText.Append(strTmp);
						}
					}

					if (convertToRMB)
					{
						if (0 == chineseText.Length) // �������ַ���ʲôҲû��
						{
							chineseText.Append("��Բ��");
						}
						// �������ַ��������"Բ"��β
						else if ("Բ" == chineseText.ToString().Substring(chineseText.Length - 1, 1))
						{
							chineseText.Append('��');
						}
					}
				}
				else if (convertToRMB) // ���"."�����һ���ַ�����ת���������
				{
					chineseText.Append("Բ��");
				}
			}
		}
		
		/// <summary>
		/// �������������Ч��
		/// </summary>
		/// <param name="numberChar">��ת���������ַ���</param>
		/// <param name="convertToRMB">�Ƿ�ת��Ϊ����Ҹ�ʽ</param>
		private static void CheckDigit(ref string numberChar, bool convertToRMB)
		{
			decimal dec;
			try
			{
				dec = decimal.Parse(numberChar);
			}
			catch (FormatException)
			{
				throw new Exception("�������ֵĸ�ʽ����ȷ��");
			}
			catch (Exception e)
			{
				throw e;
			}

			if (convertToRMB) // ���ת���������
			{
				if (dec >= 10000000000000000m)
				{
					throw new Exception("��������̫�󣬳�����Χ��");
				}
				else if (dec < 0)
				{
					throw new Exception("�����������Ϊ��ֵ��");
				}
			}
			else   // ���ת�������Ĵ�д
			{
				if (dec <= -10000000000000000m || dec >= 10000000000000000m)
				{
					throw new Exception("��������̫���̫С��������Χ��");
				}
			}
		}
		
		/// <summary>
		/// ��ȡ�����ַ����ķ���
		/// </summary>
		/// <param name="chineseText">ת������ַ���</param>
		/// <param name="numberChar">�����ַ���</param>
		/// <param name="convertToRMB">�Ƿ�ת��Ϊ����Ҹ�ʽ</param>
		protected static void ExtractSign(ref StringBuilder chineseText, ref string numberChar, bool convertToRMB)
		{
			// '+'����ǰ
			if ("+" == numberChar.Substring(0, 1))
			{
				numberChar = numberChar.Substring(1);
			}
			// '-'����ǰ
			else if ("-" == numberChar.Substring(0, 1))
			{
				if (!convertToRMB)
				{
					chineseText.Append('��');
				}
				numberChar = numberChar.Substring(1);
			}
			// '+'�����
			else if ("+" == numberChar.Substring(numberChar.Length - 1, 1))
			{
				numberChar = numberChar.Substring(0, numberChar.Length - 1);
			}
			// '-'�����
			else if ("-" == numberChar.Substring(numberChar.Length - 1, 1))
			{
				if (!convertToRMB)
				{
					chineseText.Append('��');
				}
				numberChar = numberChar.Substring(0, numberChar.Length - 1);
			}
		}

		/// <summary>
		/// ת����������
		/// </summary>
		/// <param name="strIntegral"></param>
		/// <param name="convertToRMB"></param>
		/// <returns></returns>
		protected static string ConvertIntegral(string strIntegral, bool convertToRMB)
		{
			// ȥ������ǰ�����е�'0'
			// �������ַָ�ַ�������
			char[] integral = ((long.Parse(strIntegral)).ToString()).ToCharArray();

			// �������ַ���
			StringBuilder strInt = new StringBuilder();

			int digit;
			digit = integral.Length - 1;

			// ʹ����ȷ������
			char[] chnText = convertToRMB ? chnRMBText : chnGenText;
			char[] chnDigit = convertToRMB ? chnRMBDigit : chnGenDigit;

			// ����������ֲ����������λ
			// �������λ��ʮλ����������
			int i;
			for (i = 0; i < integral.Length - 1; i++)
			{
				// �������
				strInt.Append(chnText[integral[i] - '0']);

				// �����λ
				if (0 == digit % 4)     // '��' �� '��'
				{
					if (4 == digit || 12 == digit)
					{
						strInt.Append(chnDigit[3]); // '��'
					}
					else if (8 == digit)
					{
						strInt.Append(chnDigit[4]); // '��'
					}
				}
				else         // 'ʮ'��'��'��'ǧ'
				{
					strInt.Append(chnDigit[digit % 4 - 1]);
				}

				digit--;
			}

			// �����λ������'0'
			// ����ֻ��һλ��
			// �������Ӧ����������
			if ('0' != integral[integral.Length - 1] || 1 == integral.Length)
			{
				strInt.Append(chnText[integral[i] - '0']);
			}


			// ���������ַ���
			i = 0;
			string strTemp;  // ��ʱ�洢�ַ���
			int j;    // ���ҡ���x���ṹʱ��
			bool bDoSomething; // �ҵ������򡱻����ڡ�ʱΪ��

			while (i < strInt.Length)
			{
				j = i;

				bDoSomething = false;

				// �������������ġ���x���ṹ
				while (j < strInt.Length - 1 && "��" == strInt.ToString().Substring(j, 1))
				{
					strTemp = strInt.ToString().Substring(j + 1, 1);

					// ����ǡ����򡱻��ߡ����ڡ���ֹͣ����
					if (chnDigit[3].ToString() /* ����f */ == strTemp ||
					 chnDigit[4].ToString() /* �ڻ�| */ == strTemp)
					{
						bDoSomething = true;
						break;
					}

					j += 2;
				}

				if (j != i) // ����ҵ��ǡ����򡱻��ߡ����ڡ��ġ���x���ṹ����ȫ��ɾ��
				{
					strInt = strInt.Remove(i, j - i);

					// ��������β��������治��"����"��"����"�������, 
					// ������������һ�����㡱
					if (i <= strInt.Length - 1 && !bDoSomething)
					{
						strInt = strInt.Insert(i, '��');
						i++;
					}
				}

				if (bDoSomething) // ����ҵ�"����"��"����"�ṹ
				{
					strInt = strInt.Remove(i, 1); // ȥ��'��'
					i++;
					continue;
				}

				// ָ��ÿ�ο��ƶ�2λ
				i += 2;
			}

			// ���������򡱱�ɡ����㡱��"��"
			strTemp = chnDigit[4].ToString() + chnDigit[3].ToString(); // �����ַ��������򡱻򡰃|�f��
			int index = strInt.ToString().IndexOf(strTemp);
			if (-1 != index)
			{
				if (strInt.Length - 2 != index &&  // ���"����"����ĩβ
				 (index + 2 < strInt.Length
				  && "��" != strInt.ToString().Substring(index + 2, 1))) // �������û��"��"
				{
					strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // �䡰����Ϊ�����㡱
					strInt = strInt.Insert(index + 1, "��");
				}
				else  // �����������ĩβ��������Ѿ��С��㡱
				{
					strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // �䡰����Ϊ���ڡ�
				}
			}

			if (!convertToRMB) // ���ת��Ϊһ�����Ĵ�д
			{
				// ��ͷΪ��һʮ����Ϊ��ʮ��
				if (strInt.Length > 1 && "һʮ" == strInt.ToString().Substring(0, 2))
				{
					strInt = strInt.Remove(0, 1);
				}
			}

			return strInt.ToString();
		}
		
		/// <summary>
		/// ת��С������
		/// </summary>
		/// <param name="strFractional"></param>
		/// <param name="convertToRMB"></param>
		/// <returns></returns>
		protected static string ConvertFractional(string strFractional, bool convertToRMB)
		{
			char[] fractional = strFractional.ToCharArray();

			StringBuilder strFrac = new StringBuilder();

			// �����������
			int i;
			if (convertToRMB) // ���ת��Ϊ�����
			{
				for (i = 0; i < Math.Min(2, fractional.Length); i++)
				{
					strFrac.Append(chnRMBText[fractional[i] - '0']);
					strFrac.Append(chnRMBUnit[i]);
				}

				// ��������λ�ǡ���֡���ɾ��
				if ("���" == strFrac.ToString().Substring(strFrac.Length - 2, 2))
				{
					strFrac.Remove(strFrac.Length - 2, 2);
				}

				// ����ԡ���ǡ���ͷ
				if ("���" == strFrac.ToString().Substring(0, 2))
				{
					// ���ֻʣ�¡���ǡ�
					if (2 == strFrac.Length)
					{
						strFrac.Remove(0, 2);
					}
					else // ������С�x�֡�����ɾ�����ǡ�
					{
						strFrac.Remove(1, 1);
					}
				}
			}
			else // ���ת��Ϊһ�����Ĵ�д
			{
				for (i = 0; i < fractional.Length; i++)
				{
					strFrac.Append(chnGenText[fractional[i] - '0']);
				}
			}

			return strFrac.ToString();
		}
		#endregion
	}
}