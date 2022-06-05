using System;
using System.Collections.Generic;
using System.Linq;

namespace Slid.Utils
{
	/// <summary>
	/// Реализует методы расширения для экземпляров класса <see cref="Enum"/>.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Проверяет, задан ли указанный атрибут для члена перечисления.
		/// </summary>
		/// <typeparam name="T">Тип атрибута.</typeparam>
		/// <param name="enumObj">Член перечисления.</param>
		/// <returns>Возвращает значение true, если такой атрибут присутствует, иначе - false.</returns>
		public static bool HasAttribute<T>(this Enum enumObj)
		{
			return EnumUtils.GetAttribute(enumObj, typeof(T)) != null;
		}

		/// <summary>
		/// Получает атрибут указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип атрибута.</typeparam>
		/// <param name="enumObj">Член перечисления.</param>
		/// <returns>Возвращает экземпляр атрибута, или null если такого атрибута нет.</returns>
		public static T GetAttribute<T>(this Enum enumObj)
		{
			return (T)EnumUtils.GetAttribute(enumObj, typeof(T));
		}

		/// <summary>
		/// Получает значение атрибута <see cref="System.ComponentModel.DescriptionAttribute"/> указанного для члена перечисления.
		/// </summary>
		/// <param name="member">Член перечисления.</param>
		/// <returns>Возвращает значение атрибута <see cref="System.ComponentModel.DescriptionAttribute"/> или строковое значение члена перечисления.</returns>
		public static string GetDescription(this Enum member)
		{
			return EnumUtils.GetDescription(member);
		}

		/// <summary>
		/// Получение описания всех флагов.
		/// </summary>
		/// <param name="member">Член перечисления.</param>
		/// <returns>Описание флагов перечисления.</returns>
		public static IEnumerable<string> GetFlagsDescription(this Enum member)
		{
			Array values = Enum.GetValues(member.GetType());

			foreach (Enum value in values)
			{
				if (member.HasFlag(value))
				{
					yield return value.GetDescription();
				}
			}
		}

		/// <summary>
		/// Получение описания всех флагов.
		/// </summary>
		/// <param name="member">Член перечисления.</param>
		/// <param name="separator">Разделитель описаний.</param>
		/// <returns>Описание перечислений.</returns>
		public static string GetFlagsDescription(this Enum member, string separator)
		{
			string[] result = member.GetFlagsDescription().ToArray();

			return string.Join(separator, result);
		}

		/// <summary>
		/// Получение позиции установленного флага.
		/// </summary>
		/// <param name="member">Член перечисления.</param>
		/// <returns></returns>
		public static int GetFlagSetBitPosition(this Enum member)
		{
			ulong value = Convert.ToUInt64(member);

			int bitPosition = -1;

			while (value > 0)
			{
				value >>= 1;
				++bitPosition;
			}

			return bitPosition;
		}

		///// <summary>
		///// Получает значение атрибута <see cref="ShortDescriptionAttribute"/> указанного для члена перечисления.
		///// </summary>
		///// <param name="member">Член перечисления.</param>
		///// <returns>Возвращает значение атрибута <see cref="ShortDescriptionAttribute"/> или строковое значение члена перечисления.</returns>
		//public static string GetShortDescription(this Enum member)
		//{
		//	return EnumUtils.GetShortDescription(member);
		//}

		/// <summary>
		/// Возвращает значения перечисления.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static IEnumerable<TEnum> GetValues<TEnum>()
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}
	}
}