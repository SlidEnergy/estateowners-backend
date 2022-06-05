using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Slid.Utils
{
	/// <summary>
	/// Содержит методы по работе с перечислениями.
	/// </summary>
	public static class EnumUtils
	{
		/// <summary>
		/// Получить указанный атрибут члена перечисления.
		/// </summary>
		/// <param name="enumObj">Значение перечисления.</param>
		/// <param name="attributeType">Тип атрибута.</param>
		/// <returns>Возвращает первый атрибут указанного типа или null, если атрибут не найден.</returns>
		public static object GetAttribute(Enum enumObj, Type attributeType)
		{
			if (enumObj == null)
				throw new ArgumentNullException(nameof(enumObj));

			if (!attributeType.IsSubclassOf(typeof(Attribute)))
				throw new ArgumentException("Тип должен быть атрибутом.", nameof(attributeType));

			// Получаем информацию о значении перечисления.
			FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

			if (fieldInfo == null)
				return null;

			// Получаем список атрибутов (кроме унаследованных).
			object[] attribArray = fieldInfo.GetCustomAttributes(attributeType, false);

			// Если нашли хотя бы один атрибут, то возвращаем первый.
			return attribArray.FirstOrDefault();
		}

		/// <summary>
		/// Возвращает указанный атрибут члена перечисления.
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="enumObj"></param>
		/// <returns></returns>
		public static TAttribute GetAttribute<TAttribute>(Enum enumObj)
			=> (TAttribute)GetAttribute(enumObj, typeof(TAttribute));

		/// <summary>
		/// Получить описание члена перечисления.
		/// </summary>
		/// <param name="enumObj">Значение перечисления.</param>
		/// <returns>Возвращает строку с описанием, которая содержится в атрибуте <see cref="DescriptionAttribute"/>, или имя константы, если атрибут не найден.</returns>
		public static string GetDescription(Enum enumObj)
		{
			//// Сначала попытаемся получить локализованный атрибут.

			//var localizedAttribute = GetAttribute<Localization.LocalizedDescriptionAttribute>(enumObj);

			//if (localizedAttribute != null)
			//{
			//	return localizedAttribute.Description;
			//}

			// Находим атрибут [Description], и возвращаем его значение.

			var attribute = (DescriptionAttribute)GetAttribute(enumObj, typeof(DescriptionAttribute));

			if (attribute != null)
				return attribute.Description;

			// Атрибут не найден - возвращаем текстовое представление значения перечисления.

			return enumObj.ToString();
		}

		///// <summary>
		///// Получить краткое описание члена перечисления.
		///// </summary>
		///// <param name="enumObj">Значение перечисления.</param>
		///// <returns>Возвращает строку с кратким описанием, которая содержится в атрибуте <see cref="ShortDescriptionAttribute"/>, или имя константы, если атрибут не найден.</returns>
		//public static string GetShortDescription(Enum enumObj)
		//{
		//	// Находим атрибут [ShortDescription], и возвращаем его значение.

		//	var attribute = (ShortDescriptionAttribute)GetAttribute(enumObj, typeof(ShortDescriptionAttribute));

		//	if (attribute != null)
		//		return attribute.ShortDescription;

		//	// Атрибут не найден - возвращаем текстовое представление значения перечисления.

		//	return enumObj.ToString();
		//}

		/// <summary>
		/// Возвращает список
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static IEnumerable<TEnum> GetValues<TEnum>() => (TEnum[])Enum.GetValues(typeof(TEnum));

		/// <summary>
		/// Возвращает список флагов в перечислении.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="flags"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TEnum> DeconstructFlags<TEnum>(this TEnum flags, Predicate<TEnum> selector)
			where TEnum : Enum
		{
			foreach (TEnum itemValue in GetValues<TEnum>()
				.Where(x => selector(x)))
			{
				if (flags.HasFlag(itemValue))
				{
					yield return itemValue;
				}
			}
		}

		/// <summary>
		/// Формирует флаги из списка элементов.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static TEnum ConstructFlags<TEnum>(this IEnumerable<TEnum> items)
			where TEnum : Enum
		{
			int result = 0;

			foreach (var item in items)
			{
				result |= Convert.ToInt32(item);
			}

			if (typeof(TEnum).GetEnumUnderlyingType() == typeof(byte))
			{
				return (TEnum)((object)(byte)result);
			}
			else if (typeof(TEnum).GetEnumUnderlyingType() == typeof(short))
			{
				return (TEnum)((object)(short)result);
			}
			else
			{
				return (TEnum)(object)result;
			}
		}
	}
}
