﻿/*
 * Copyright (C) 2011 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PhoneNumbers.Test
{
    /**
    * UnitTests for AreaCodeMap.java
    *
    * @author Shaopeng Jia
    */
    public class TestAreaCodeMap
    {
        private readonly AreaCodeMap areaCodeMapForUS = new AreaCodeMap();
        private readonly AreaCodeMap areaCodeMapForIT = new AreaCodeMap();
        
        public void SetupFixture()
        {
            SortedDictionary<int, String> sortedMap = new SortedDictionary<int, String>();
            sortedMap[1212] = "New York";
            sortedMap[1480] = "Arizona";
            sortedMap[1650] = "California";
            sortedMap[1907] = "Alaska";
            sortedMap[1201664] = "Westwood, NJ";
            sortedMap[1480893] = "Phoenix, AZ";
            sortedMap[1501372] = "Little Rock, AR";
            sortedMap[1626308] = "Alhambra, CA";
            sortedMap[1650345] = "San Mateo, CA";
            sortedMap[1867993] = "Dawson, YT";
            sortedMap[1972480] = "Richardson, TX";
            areaCodeMapForUS.readAreaCodeMap(sortedMap);

            sortedMap = new SortedDictionary<int, String>();
            sortedMap[3902] = "Milan";
            sortedMap[3906] = "Rome";
            sortedMap[39010] = "Genoa";
            sortedMap[390131] = "Alessandria";
            sortedMap[390321] = "Novara";
            sortedMap[390975] = "Potenza";
            areaCodeMapForIT.readAreaCodeMap(sortedMap);
        }

        private static SortedDictionary<int, String> createDefaultStorageMapCandidate()
        {
            var sortedMap = new SortedDictionary<int, String>();
            // Make the area codes bigger to store them using integer.
            sortedMap[121212345] = "New York";
            sortedMap[148034434] = "Arizona";
            return sortedMap;
        }

        private static SortedDictionary<int, String> createFlyweightStorageMapCandidate()
        {
            var sortedMap = new SortedDictionary<int, String>();
            sortedMap[1212] = "New York";
            sortedMap[1213] = "New York";
            sortedMap[1214] = "New York";
            sortedMap[1480] = "Arizona";
            return sortedMap;
        }

        [Fact]
        public void TestGetSmallerMapStorageChoosesDefaultImpl()
        {
            AreaCodeMapStorageStrategy mapStorage =
                new AreaCodeMap().getSmallerMapStorage(createDefaultStorageMapCandidate());
            Assert.False(mapStorage.GetType() == typeof(FlyweightMapStorage));
        }

        [Fact]
        public void TestGetSmallerMapStorageChoosesFlyweightImpl()
        {
            AreaCodeMapStorageStrategy mapStorage =
                new AreaCodeMap().getSmallerMapStorage(createFlyweightStorageMapCandidate());
            Assert.True(mapStorage.GetType() == typeof(FlyweightMapStorage));
        }

        [Fact]
        public void TestLookupInvalidNumber_US()
        {
            // central office code cannot start with 1.
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2121234567L).Build();
            Assert.Equal("New York", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_NJ()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2016641234L).Build();
            Assert.Equal("Westwood, NJ", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_NY()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2126641234L).Build();
            Assert.Equal("New York", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_CA_1()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(6503451234L).Build();
            Assert.Equal("San Mateo, CA", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_CA_2()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(6502531234L).Build();
            Assert.Equal("California", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumberFound_TX()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(9724801234L).Build();
            Assert.Equal("Richardson, TX", areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumberNotFound_TX()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(9724811234L).Build();
            Assert.Null(areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_CH()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(41).SetNationalNumber(446681300L).Build();
            Assert.Null(areaCodeMapForUS.Lookup(number));
        }

        [Fact]
        public void TestLookupNumber_IT()
        {
            var number = new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(212345678L).SetItalianLeadingZero(true)
                .Build();
            Assert.Equal("Milan", areaCodeMapForIT.Lookup(number));

            number = new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(612345678L).SetItalianLeadingZero(true)
                .Build();
            Assert.Equal("Rome", areaCodeMapForIT.Lookup(number));

            number = new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(3211234L).SetItalianLeadingZero(true)
                .Build();
            Assert.Equal("Novara", areaCodeMapForIT.Lookup(number));

            // A mobile number
            number = new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(321123456L).SetItalianLeadingZero(false)
                .Build();
            Assert.Null(areaCodeMapForIT.Lookup(number));

            // An invalid number (too short)
            number = new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(321123L).SetItalianLeadingZero(true)
                .Build();
            Assert.Equal("Novara", areaCodeMapForIT.Lookup(number));
        }
    }
}
