﻿using System.Collections.Generic;
using System.Windows;

namespace StatisticsAnalysisTool.Common
{
    using Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Text;

    public class ItemController
    {
        public static string LocalizedName(LocalizedNames localizedNames, string currentLanguage = null, string alternativeName = "NO_ITEM_NAME")
        {
            if (localizedNames == null)
                return "";

            if (string.IsNullOrEmpty(currentLanguage))
                currentLanguage = LanguageController.CurrentCultureInfo.TextInfo.CultureName.ToUpper();

            switch (FrequentlyValues.GameLanguages.FirstOrDefault(x => string.Equals(x.Value, currentLanguage, StringComparison.CurrentCultureIgnoreCase)).Key)
            {
                case FrequentlyValues.GameLanguage.UnitedStates:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.EnUs));
                case FrequentlyValues.GameLanguage.Germany:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.DeDe));
                case FrequentlyValues.GameLanguage.Russia:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.RuRu));
                case FrequentlyValues.GameLanguage.Poland:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.PlPl));
                case FrequentlyValues.GameLanguage.Brazil:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.PtBr));
                case FrequentlyValues.GameLanguage.France:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.FrFr));
                case FrequentlyValues.GameLanguage.Spain:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.EsEs));
                case FrequentlyValues.GameLanguage.Chinese:
                    return Encoding.UTF8.GetString(Encoding.Default.GetBytes(localizedNames.ZhCn));
                default:
                    return alternativeName;
            }
        }

        public static FrequentlyValues.ItemTier GetItemTier(string uniqueName) => FrequentlyValues.ItemTiers.FirstOrDefault(x => x.Value == uniqueName.Split('_')[0]).Key;

        public static FrequentlyValues.ItemLevel GetItemLevel(string uniqueName)
        {
            if (!uniqueName.Contains("@"))
                return FrequentlyValues.ItemLevel.Level0;

            if (int.TryParse(uniqueName.Split('@')[1], out int number))
                return FrequentlyValues.ItemLevels.First(x => x.Value == number).Key;
            return FrequentlyValues.ItemLevel.Level0;
        }

        public static int GetQuality(FrequentlyValues.ItemQuality value) => FrequentlyValues.ItemQualities.FirstOrDefault(x => x.Key == value).Value;

        public static FrequentlyValues.ItemQuality GetQuality(int value) => FrequentlyValues.ItemQualities.FirstOrDefault(x => x.Value == value).Key;

        public static void AddLocalizedName(ref ItemData itemData, JObject parsedObject)
        {
            foreach (var language in Enum.GetValues(typeof(FrequentlyValues.GameLanguage)).Cast<FrequentlyValues.GameLanguage>())
            {
                var cultureCode = FrequentlyValues.GameLanguages.FirstOrDefault(x => x.Key == language).Value;

                if (parsedObject["localizedNames"]?[cultureCode] != null)
                    itemData.LocalizedNames.Add(new ItemData.KeyValueStruct() { Key = cultureCode, Value = parsedObject["localizedNames"][cultureCode].ToString() });
            }
        }

        public static Style LocationStyle(Location location) {
            switch (location)
            {
                case Location.Caerleon:
                    return Application.Current.FindResource("CaerleonStyle") as Style;
                case Location.Thetford:
                    return Application.Current.FindResource("ThetfordStyle") as Style;
                case Location.Bridgewatch:
                    return Application.Current.FindResource("BridgewatchStyle") as Style;
                case Location.Martlock:
                    return Application.Current.FindResource("MartlockStyle") as Style;
                case Location.Lymhurst:
                    return Application.Current.FindResource("LymhurstStyle") as Style;
                case Location.FortSterling:
                    return Application.Current.FindResource("FortSterlingStyle") as Style;
                case Location.ArthursRest:
                    return Application.Current.FindResource("ArthursRestStyle") as Style;
                case Location.MerlynsRest:
                    return Application.Current.FindResource("MerlynsRestStyle") as Style;
                case Location.MorganasRest:
                    return Application.Current.FindResource("MorganasRestStyle") as Style;
                default:
                    return Application.Current.FindResource("DefaultCityStyle") as Style;
            }
        }

        public static Style GetStyleByTimestamp(DateTime value)
        {
            if (value.Date == DateTime.MinValue.Date)
                return Application.Current.FindResource("ListView.Grid.Label.Date.NoValue") as Style;

            if (value.AddHours(8) < DateTime.Now.ToUniversalTime().AddHours(-1))
                return Application.Current.FindResource("ListView.Grid.Label.Date.ToOldFirst") as Style;

            if (value.AddHours(4) < DateTime.Now.ToUniversalTime().AddHours(-1))
                return Application.Current.FindResource("ListView.Grid.Label.Date.ToOldSecond") as Style;

            if (value.AddHours(2) < DateTime.Now.ToUniversalTime().AddHours(-1))
                return Application.Current.FindResource("ListView.Grid.Label.Date.ToOldThird") as Style;

            return Application.Current.FindResource("ListView.Grid.Label.Date.Normal") as Style;
        }

        public static Style PriceStyle(bool bestSellMinPrice) {
            switch (bestSellMinPrice)
            {
                case true:
                    return Application.Current.FindResource("ListView.Grid.StackPanel.Label.BestPrice") as Style;
                case false:
                    return Application.Current.FindResource("ListView.Grid.StackPanel.Label.Price") as Style;
                default:
                    return Application.Current.FindResource("ListView.Grid.StackPanel.Label.Price") as Style;
            }
        }

        public static ulong GetMinPrice(List<ulong> list)
        {
            var min = ulong.MaxValue;
            foreach (var value in list)
            {
                if (value == 0)
                    continue;

                if (value < min)
                    min = value;
            }

            return min;
        }
    }
}