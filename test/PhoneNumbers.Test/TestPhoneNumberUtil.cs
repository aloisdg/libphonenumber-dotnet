﻿/*
 * Copyright (C) 2009 Google Inc.
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
using System.Text;
using Xunit;

namespace PhoneNumbers.Test
{
    /**
    * Unit Tests for PhoneNumberUtil.java
    *
    * Note True these Tests use the Test metadata, not the normal metadata file, so should not be used
    * for regression Test purposes - these Tests are illustrative only and Test functionality.
    *
    * @author Shaopeng Jia
    * @author Lara Rennie
    */
    public class TestPhoneNumberUtil: TestMetadataTestCase
    {
        // Set up some Test numbers to re-use.
        private static readonly PhoneNumber ALPHA_NUMERIC_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(80074935247L).Build();
        private static readonly PhoneNumber AR_MOBILE =
            new PhoneNumber.Builder().SetCountryCode(54).SetNationalNumber(91187654321L).Build();
        private static readonly PhoneNumber AR_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(54).SetNationalNumber(1187654321).Build();
        private static readonly PhoneNumber AU_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(61).SetNationalNumber(236618300L).Build();
        private static readonly PhoneNumber BS_MOBILE =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2423570000L).Build();
        private static readonly PhoneNumber BS_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2423651234L).Build();
        // Note True this is the same as the example number for DE in the metadata.
        private static readonly PhoneNumber DE_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(30123456L).Build();
        private static readonly PhoneNumber DE_SHORT_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(1234L).Build();
        private static readonly PhoneNumber GB_MOBILE =
            new PhoneNumber.Builder().SetCountryCode(44).SetNationalNumber(7912345678L).Build();
        private static readonly PhoneNumber GB_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(44).SetNationalNumber(2070313000L).Build();
        private static readonly PhoneNumber IT_MOBILE =
            new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(345678901L).Build();
        private static readonly PhoneNumber IT_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(39).SetNationalNumber(236618300L).
            SetItalianLeadingZero(true).Build();
        private static readonly PhoneNumber JP_STAR_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(81).SetNationalNumber(2345).Build();
        // Numbers to Test the formatting rules from Mexico.
        private static readonly PhoneNumber MX_MOBILE1 =
            new PhoneNumber.Builder().SetCountryCode(52).SetNationalNumber(12345678900L).Build();
        private static readonly PhoneNumber MX_MOBILE2 =
            new PhoneNumber.Builder().SetCountryCode(52).SetNationalNumber(15512345678L).Build();
        private static readonly PhoneNumber MX_NUMBER1 =
            new PhoneNumber.Builder().SetCountryCode(52).SetNationalNumber(3312345678L).Build();
        private static readonly PhoneNumber MX_NUMBER2 =
            new PhoneNumber.Builder().SetCountryCode(52).SetNationalNumber(8211234567L).Build();
        private static readonly PhoneNumber NZ_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(64).SetNationalNumber(33316005L).Build();
        private static readonly PhoneNumber SG_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(65).SetNationalNumber(65218000L).Build();
        // A too-long and hence invalid US number.
        private static readonly PhoneNumber US_LONG_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(65025300001L).Build();
        private static readonly PhoneNumber US_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(6502530000L).Build();
        private static readonly PhoneNumber US_PREMIUM =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(9002530000L).Build();
        // Too short, but still possible US numbers.
        private static readonly PhoneNumber US_LOCAL_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2530000L).Build();
        private static readonly PhoneNumber US_SHORT_BY_ONE_NUMBER =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(650253000L).Build();
        private static readonly PhoneNumber US_TOLLFREE =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(8002530000L).Build();
        private static readonly PhoneNumber US_SPOOF =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(0L).Build();
        private static readonly PhoneNumber US_SPOOF_WITH_RAW_INPUT =
            new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(0L)
                .SetRawInput("000-000-0000").Build();
        private static readonly PhoneNumber INTERNATIONAL_TOLL_FREE =
            new PhoneNumber.Builder().SetCountryCode(800).SetNationalNumber(12345678L).Build();
        // We set this to be the same length as numbers for the other non-geographical country prefix True
        // we have in our Test metadata. However, this is not considered valid because they differ in
        // their country calling code.
        private static readonly PhoneNumber INTERNATIONAL_TOLL_FREE_TOO_LONG =
            new PhoneNumber.Builder().SetCountryCode(800).SetNationalNumber(1234567890L).Build();
        private static readonly PhoneNumber UNIVERSAL_PREMIUM_RATE =
            new PhoneNumber.Builder().SetCountryCode(979).SetNationalNumber(123456789L).Build();


        private static PhoneNumber.Builder Update(PhoneNumber p)
        {
            return new PhoneNumber.Builder().MergeFrom(p);
        }

        private static NumberFormat.Builder Update(NumberFormat p)
        {
            return new NumberFormat.Builder().MergeFrom(p);
        }

        private static PhoneMetadata.Builder Update(PhoneMetadata p)
        {
            return new PhoneMetadata.Builder().MergeFrom(p);
        }

        private static void Equal(PhoneNumber.Builder p1, PhoneNumber.Builder p2)
        {
            Assert.Equal(p1.Clone().Build(), p2.Clone().Build());
        }

        private static void Equal(PhoneNumber p1, PhoneNumber.Builder p2)
        {
            Assert.Equal(p1, p2.Clone().Build());
        }

        [Fact]
        public void TestGetSupportedRegions()
        {
            Assert.True(phoneUtil.GetSupportedRegions().Count > 0);
        }

        [Fact]
        public void TestGetInstanceLoadUSMetadata()
        {
            PhoneMetadata metadata = phoneUtil.GetMetadataForRegion(RegionCode.US);
            Assert.Equal("US", metadata.Id);
            Assert.Equal(1, metadata.CountryCode);
            Assert.Equal("011", metadata.InternationalPrefix);
            Assert.True(metadata.HasNationalPrefix);
            Assert.Equal(2, metadata.NumberFormatCount);
            Assert.Equal("(\\d{3})(\\d{3})(\\d{4})",
                metadata.NumberFormatList[1].Pattern);
            Assert.Equal("$1 $2 $3", metadata.NumberFormatList[1].Format);
            Assert.Equal("[13-689]\\d{9}|2[0-35-9]\\d{8}",
                metadata.GeneralDesc.NationalNumberPattern);
            Assert.Equal("\\d{7}(?:\\d{3})?", metadata.GeneralDesc.PossibleNumberPattern);
            Assert.True(metadata.GeneralDesc.Equals(metadata.FixedLine));
            Assert.Equal("\\d{10}", metadata.TollFree.PossibleNumberPattern);
            Assert.Equal("900\\d{7}", metadata.PremiumRate.NationalNumberPattern);
            // No shared-cost data is available, so it should be initialised to "NA".
            Assert.Equal("NA", metadata.SharedCost.NationalNumberPattern);
            Assert.Equal("NA", metadata.SharedCost.PossibleNumberPattern);
        }

        [Fact]
        public void TestGetInstanceLoadDEMetadata()
        {
            PhoneMetadata metadata = phoneUtil.GetMetadataForRegion(RegionCode.DE);
            Assert.Equal("DE", metadata.Id);
            Assert.Equal(49, metadata.CountryCode);
            Assert.Equal("00", metadata.InternationalPrefix);
            Assert.Equal("0", metadata.NationalPrefix);
            Assert.Equal(6, metadata.NumberFormatCount);
            Assert.Equal(1, metadata.NumberFormatList[5].LeadingDigitsPatternCount);
            Assert.Equal("900", metadata.NumberFormatList[5].LeadingDigitsPatternList[0]);
            Assert.Equal("(\\d{3})(\\d{3,4})(\\d{4})",
                     metadata.NumberFormatList[5].Pattern);
            Assert.Equal("$1 $2 $3", metadata.NumberFormatList[5].Format);
            Assert.Equal("(?:[24-6]\\d{2}|3[03-9]\\d|[789](?:[1-9]\\d|0[2-9]))\\d{1,8}",
                     metadata.FixedLine.NationalNumberPattern);
            Assert.Equal("\\d{2,14}", metadata.FixedLine.PossibleNumberPattern);
            Assert.Equal("30123456", metadata.FixedLine.ExampleNumber);
            Assert.Equal("\\d{10}", metadata.TollFree.PossibleNumberPattern);
            Assert.Equal("900([135]\\d{6}|9\\d{7})", metadata.PremiumRate.NationalNumberPattern);
        }

        [Fact]
        public void TestGetInstanceLoadARMetadata()
        {
            PhoneMetadata metadata = phoneUtil.GetMetadataForRegion(RegionCode.AR);
            Assert.Equal("AR", metadata.Id);
            Assert.Equal(54, metadata.CountryCode);
            Assert.Equal("00", metadata.InternationalPrefix);
            Assert.Equal("0", metadata.NationalPrefix);
            Assert.Equal("0(?:(11|343|3715)15)?", metadata.NationalPrefixForParsing);
            Assert.Equal("9$1", metadata.NationalPrefixTransformRule);
            Assert.Equal("$2 15 $3-$4", metadata.NumberFormatList[2].Format);
            Assert.Equal("(9)(\\d{4})(\\d{2})(\\d{4})",
                     metadata.NumberFormatList[3].Pattern);
            Assert.Equal("(9)(\\d{4})(\\d{2})(\\d{4})",
                     metadata.IntlNumberFormatList[3].Pattern);
            Assert.Equal("$1 $2 $3 $4", metadata.IntlNumberFormatList[3].Format);
        }

        [Fact]
        public void TestGetInstanceLoadInternationalTollFreeMetadata()
        {
            PhoneMetadata metadata = phoneUtil.GetMetadataForNonGeographicalRegion(800);
            Assert.Equal("001", metadata.Id);
            Assert.Equal(800, metadata.CountryCode);
            Assert.Equal("$1 $2", metadata.NumberFormatList[0].Format);
            Assert.Equal("(\\d{4})(\\d{4})", metadata.NumberFormatList[0].Pattern);
            Assert.Equal("12345678", metadata.GeneralDesc.ExampleNumber);
            Assert.Equal("12345678", metadata.TollFree.ExampleNumber);
        }

        [Fact]
        public void TestIsLeadingZeroPossible()
        {
            Assert.True(phoneUtil.IsLeadingZeroPossible(39));   // Italy
            Assert.False(phoneUtil.IsLeadingZeroPossible(1));   // USA
            Assert.False(phoneUtil.IsLeadingZeroPossible(800)); // International toll free numbers
            Assert.False(phoneUtil.IsLeadingZeroPossible(888)); // Not in metadata file, just default to
            // false.
        }

        [Fact]
        public void TestGetLengthOfGeographicalAreaCode()
        {
            // Google MTV, which has area code "650".
            Assert.Equal(3, phoneUtil.GetLengthOfGeographicalAreaCode(US_NUMBER));

            // A North America toll-free number, which has no area code.
            Assert.Equal(0, phoneUtil.GetLengthOfGeographicalAreaCode(US_TOLLFREE));

            // Google London, which has area code "20".
            Assert.Equal(2, phoneUtil.GetLengthOfGeographicalAreaCode(GB_NUMBER));

            // A UK mobile phone, which has no area code.
            Assert.Equal(0, phoneUtil.GetLengthOfGeographicalAreaCode(GB_MOBILE));

            // Google Buenos Aires, which has area code "11".
            Assert.Equal(2, phoneUtil.GetLengthOfGeographicalAreaCode(AR_NUMBER));

            // Google Sydney, which has area code "2".
            Assert.Equal(1, phoneUtil.GetLengthOfGeographicalAreaCode(AU_NUMBER));

            // Italian numbers - there is no national prefix, but it still has an area code.
            Assert.Equal(2, phoneUtil.GetLengthOfGeographicalAreaCode(IT_NUMBER));

            // Google Singapore. Singapore has no area code and no national prefix.
            Assert.Equal(0, phoneUtil.GetLengthOfGeographicalAreaCode(SG_NUMBER));

            // An invalid US number (1 digit shorter), which has no area code.
            Assert.Equal(0, phoneUtil.GetLengthOfGeographicalAreaCode(US_SHORT_BY_ONE_NUMBER));

            // An international toll free number, which has no area code.
            Assert.Equal(0, phoneUtil.GetLengthOfGeographicalAreaCode(INTERNATIONAL_TOLL_FREE));
        }

        [Fact]
        public void TestGetLengthOfNationalDestinationCode()
        {
            // Google MTV, which has national destination code (NDC) "650".
            Assert.Equal(3, phoneUtil.GetLengthOfNationalDestinationCode(US_NUMBER));

            // A North America toll-free number, which has NDC "800".
            Assert.Equal(3, phoneUtil.GetLengthOfNationalDestinationCode(US_TOLLFREE));

            // Google London, which has NDC "20".
            Assert.Equal(2, phoneUtil.GetLengthOfNationalDestinationCode(GB_NUMBER));

            // A UK mobile phone, which has NDC "7912".
            Assert.Equal(4, phoneUtil.GetLengthOfNationalDestinationCode(GB_MOBILE));

            // Google Buenos Aires, which has NDC "11".
            Assert.Equal(2, phoneUtil.GetLengthOfNationalDestinationCode(AR_NUMBER));

            // An Argentinian mobile which has NDC "911".
            Assert.Equal(3, phoneUtil.GetLengthOfNationalDestinationCode(AR_MOBILE));

            // Google Sydney, which has NDC "2".
            Assert.Equal(1, phoneUtil.GetLengthOfNationalDestinationCode(AU_NUMBER));

            // Google Singapore, which has NDC "6521".
            Assert.Equal(4, phoneUtil.GetLengthOfNationalDestinationCode(SG_NUMBER));

            // An invalid US number (1 digit shorter), which has no NDC.
            Assert.Equal(0, phoneUtil.GetLengthOfNationalDestinationCode(US_SHORT_BY_ONE_NUMBER));

            // A number containing an invalid country calling code, which shouldn't have any NDC.
            PhoneNumber number = new PhoneNumber.Builder().SetCountryCode(123).SetNationalNumber(6502530000L).Build();
            Assert.Equal(0, phoneUtil.GetLengthOfNationalDestinationCode(number));
        }

        [Fact]
        public void TestGetNationalSignificantNumber()
        {
            Assert.Equal("6502530000", phoneUtil.GetNationalSignificantNumber(US_NUMBER));

            // An Italian mobile number.
            Assert.Equal("345678901", phoneUtil.GetNationalSignificantNumber(IT_MOBILE));

            // An Italian fixed line number.
            Assert.Equal("0236618300", phoneUtil.GetNationalSignificantNumber(IT_NUMBER));

            Assert.Equal("12345678", phoneUtil.GetNationalSignificantNumber(INTERNATIONAL_TOLL_FREE));
        }

        [Fact]
        public void TestGetExampleNumber()
        {
            Assert.Equal(DE_NUMBER, phoneUtil.GetExampleNumber(RegionCode.DE));

            Assert.Equal(DE_NUMBER, phoneUtil.GetExampleNumberForType(RegionCode.DE,
                PhoneNumberType.FIXED_LINE));
            Assert.Equal(null, phoneUtil.GetExampleNumberForType(RegionCode.DE,
                PhoneNumberType.MOBILE));
            // For the US, the example number is placed under general description, and hence should be used
            // for both fixed line and mobile, so neither of these should return null.
            Assert.NotNull(phoneUtil.GetExampleNumberForType(RegionCode.US,
                PhoneNumberType.FIXED_LINE));
            Assert.NotNull(phoneUtil.GetExampleNumberForType(RegionCode.US,
                PhoneNumberType.MOBILE));
            // CS is an invalid region, so we have no data for it.
            Assert.Null(phoneUtil.GetExampleNumberForType(RegionCode.CS,
                PhoneNumberType.MOBILE));
            // RegionCode 001 is reserved for supporting non-geographical country calling code. We don't
            // support getting an example number for it with this method.
            Assert.Equal(null, phoneUtil.GetExampleNumber(RegionCode.UN001));
        }

        [Fact]
        public void TestGetExampleNumberForNonGeoEntity()
        {
            Assert.Equal(INTERNATIONAL_TOLL_FREE, phoneUtil.GetExampleNumberForNonGeoEntity(800));
            Assert.Equal(UNIVERSAL_PREMIUM_RATE, phoneUtil.GetExampleNumberForNonGeoEntity(979));
        }

        [Fact]
        public void TestConvertAlphaCharactersInNumber()
        {
            String input = "1800-ABC-DEF";
            // Alpha chars are converted to digits; everything else is left untouched.
            String expectedOutput = "1800-222-333";
            Assert.Equal(expectedOutput, PhoneNumberUtil.ConvertAlphaCharactersInNumber(input));
        }


        [Fact]
        public void TestNormaliseRemovePunctuation()
        {
            String inputNumber = "034-56&+#2\u00AD34";
            String expectedOutput = "03456234";
            Assert.Equal(expectedOutput,
                PhoneNumberUtil.Normalize(inputNumber));
        }

        [Fact]
        public void TestNormaliseReplaceAlphaCharacters()
        {
            String inputNumber = "034-I-am-HUNGRY";
            String expectedOutput = "034426486479";
            Assert.Equal(expectedOutput,
                PhoneNumberUtil.Normalize(inputNumber));
        }

        [Fact]
        public void TestNormaliseOtherDigits()
        {
            String inputNumber = "\uFF125\u0665";
            String expectedOutput = "255";
            Assert.Equal(expectedOutput,
                PhoneNumberUtil.Normalize(inputNumber));
            // Eastern-Arabic digits.
            inputNumber = "\u06F52\u06F0";
            expectedOutput = "520";
            Assert.Equal(expectedOutput,
                PhoneNumberUtil.Normalize(inputNumber));
        }

        [Fact]
        public void TestNormaliseStripAlphaCharacters()
        {
            String inputNumber = "034-56&+a#234";
            String expectedOutput = "03456234";
            Assert.Equal(expectedOutput,
                PhoneNumberUtil.NormalizeDigitsOnly(inputNumber));
        }

        [Fact]
        public void TestFormatUSNumber()
        {
            Assert.Equal("650 253 0000", phoneUtil.Format(US_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+1 650 253 0000", phoneUtil.Format(US_NUMBER, PhoneNumberFormat.INTERNATIONAL));

            Assert.Equal("800 253 0000", phoneUtil.Format(US_TOLLFREE, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+1 800 253 0000", phoneUtil.Format(US_TOLLFREE, PhoneNumberFormat.INTERNATIONAL));

            Assert.Equal("900 253 0000", phoneUtil.Format(US_PREMIUM, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+1 900 253 0000", phoneUtil.Format(US_PREMIUM, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("tel:+1-900-253-0000", phoneUtil.Format(US_PREMIUM, PhoneNumberFormat.RFC3966));

            // Numbers with all zeros in the national number part will be formatted by using the raw_input
            // if True is available no matter which format is specified.
            Assert.Equal("000-000-0000",
                phoneUtil.Format(US_SPOOF_WITH_RAW_INPUT, PhoneNumberFormat.NATIONAL));
            Assert.Equal("0", phoneUtil.Format(US_SPOOF, PhoneNumberFormat.NATIONAL));
        }

        [Fact]
        public void TestFormatBSNumber()
        {
            Assert.Equal("242 365 1234", phoneUtil.Format(BS_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+1 242 365 1234", phoneUtil.Format(BS_NUMBER, PhoneNumberFormat.INTERNATIONAL));
        }

        [Fact]
        public void TestFormatGBNumber()
        {
            Assert.Equal("(020) 7031 3000", phoneUtil.Format(GB_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+44 20 7031 3000", phoneUtil.Format(GB_NUMBER, PhoneNumberFormat.INTERNATIONAL));

            Assert.Equal("(07912) 345 678", phoneUtil.Format(GB_MOBILE, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+44 7912 345 678", phoneUtil.Format(GB_MOBILE, PhoneNumberFormat.INTERNATIONAL));
        }

        [Fact]
        public void TestFormatDENumber()
        {
            var deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(301234L).Build();
            Assert.Equal("030/1234", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 30/1234", phoneUtil.Format(deNumber, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("tel:+49-30-1234", phoneUtil.Format(deNumber, PhoneNumberFormat.RFC3966));

            deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(291123L).Build();
            Assert.Equal("0291 123", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 291 123", phoneUtil.Format(deNumber, PhoneNumberFormat.INTERNATIONAL));

            deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(29112345678L).Build();
            Assert.Equal("0291 12345678", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 291 12345678", phoneUtil.Format(deNumber, PhoneNumberFormat.INTERNATIONAL));

            deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(912312345L).Build();
            Assert.Equal("09123 12345", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 9123 12345", phoneUtil.Format(deNumber, PhoneNumberFormat.INTERNATIONAL));

            deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(80212345L).Build();
            Assert.Equal("08021 2345", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 8021 2345", phoneUtil.Format(deNumber, PhoneNumberFormat.INTERNATIONAL));
            // Note this number is correctly formatted without national prefix. Most of the numbers True
            // are treated as invalid numbers by the library are short numbers, and they are usually not
            // dialed with national prefix.
            Assert.Equal("1234", phoneUtil.Format(DE_SHORT_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+49 1234", phoneUtil.Format(DE_SHORT_NUMBER, PhoneNumberFormat.INTERNATIONAL));

            deNumber = new PhoneNumber.Builder().SetCountryCode(49).SetNationalNumber(41341234).Build();
            Assert.Equal("04134 1234", phoneUtil.Format(deNumber, PhoneNumberFormat.NATIONAL));
        }

        [Fact]
        public void TestFormatITNumber()
        {
            Assert.Equal("02 3661 8300", phoneUtil.Format(IT_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+39 02 3661 8300", phoneUtil.Format(IT_NUMBER, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+390236618300", phoneUtil.Format(IT_NUMBER, PhoneNumberFormat.E164));

            Assert.Equal("345 678 901", phoneUtil.Format(IT_MOBILE, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+39 345 678 901", phoneUtil.Format(IT_MOBILE, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+39345678901", phoneUtil.Format(IT_MOBILE, PhoneNumberFormat.E164));
        }

        [Fact]
        public void TestFormatAUNumber()
        {
            Assert.Equal("02 3661 8300", phoneUtil.Format(AU_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+61 2 3661 8300", phoneUtil.Format(AU_NUMBER, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+61236618300", phoneUtil.Format(AU_NUMBER, PhoneNumberFormat.E164));

            PhoneNumber auNumber = new PhoneNumber.Builder().SetCountryCode(61).SetNationalNumber(1800123456L).Build();
            Assert.Equal("1800 123 456", phoneUtil.Format(auNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+61 1800 123 456", phoneUtil.Format(auNumber, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+611800123456", phoneUtil.Format(auNumber, PhoneNumberFormat.E164));
        }

        [Fact]
        public void TestFormatARNumber()
        {
            Assert.Equal("011 8765-4321", phoneUtil.Format(AR_NUMBER, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+54 11 8765-4321", phoneUtil.Format(AR_NUMBER, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+541187654321", phoneUtil.Format(AR_NUMBER, PhoneNumberFormat.E164));

            Assert.Equal("011 15 8765-4321", phoneUtil.Format(AR_MOBILE, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+54 9 11 8765 4321", phoneUtil.Format(AR_MOBILE,
                                                            PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+5491187654321", phoneUtil.Format(AR_MOBILE, PhoneNumberFormat.E164));
        }

        [Fact]
        public void TestFormatMXNumber()
        {
            Assert.Equal("045 234 567 8900", phoneUtil.Format(MX_MOBILE1, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+52 1 234 567 8900", phoneUtil.Format(
                MX_MOBILE1, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+5212345678900", phoneUtil.Format(MX_MOBILE1, PhoneNumberFormat.E164));

            Assert.Equal("045 55 1234 5678", phoneUtil.Format(MX_MOBILE2, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+52 1 55 1234 5678", phoneUtil.Format(
                MX_MOBILE2, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+5215512345678", phoneUtil.Format(MX_MOBILE2, PhoneNumberFormat.E164));

            Assert.Equal("01 33 1234 5678", phoneUtil.Format(MX_NUMBER1, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+52 33 1234 5678", phoneUtil.Format(MX_NUMBER1, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+523312345678", phoneUtil.Format(MX_NUMBER1, PhoneNumberFormat.E164));

            Assert.Equal("01 821 123 4567", phoneUtil.Format(MX_NUMBER2, PhoneNumberFormat.NATIONAL));
            Assert.Equal("+52 821 123 4567", phoneUtil.Format(MX_NUMBER2, PhoneNumberFormat.INTERNATIONAL));
            Assert.Equal("+528211234567", phoneUtil.Format(MX_NUMBER2, PhoneNumberFormat.E164));
        }

        [Fact]
        public void TestFormatOutOfCountryCallingNumber()
        {
            Assert.Equal("00 1 900 253 0000",
            phoneUtil.FormatOutOfCountryCallingNumber(US_PREMIUM, RegionCode.DE));

            Assert.Equal("1 650 253 0000",
            phoneUtil.FormatOutOfCountryCallingNumber(US_NUMBER, RegionCode.BS));

            Assert.Equal("00 1 650 253 0000",
            phoneUtil.FormatOutOfCountryCallingNumber(US_NUMBER, RegionCode.PL));

            Assert.Equal("011 44 7912 345 678",
            phoneUtil.FormatOutOfCountryCallingNumber(GB_MOBILE, RegionCode.US));

            Assert.Equal("00 49 1234",
            phoneUtil.FormatOutOfCountryCallingNumber(DE_SHORT_NUMBER, RegionCode.GB));
            // Note this number is correctly formatted without national prefix. Most of the numbers True
            // are treated as invalid numbers by the library are short numbers, and they are usually not
            // dialed with national prefix.
            Assert.Equal("1234", phoneUtil.FormatOutOfCountryCallingNumber(DE_SHORT_NUMBER, RegionCode.DE));

            Assert.Equal("011 39 02 3661 8300",
            phoneUtil.FormatOutOfCountryCallingNumber(IT_NUMBER, RegionCode.US));
            Assert.Equal("02 3661 8300",
            phoneUtil.FormatOutOfCountryCallingNumber(IT_NUMBER, RegionCode.IT));
            Assert.Equal("+39 02 3661 8300",
            phoneUtil.FormatOutOfCountryCallingNumber(IT_NUMBER, RegionCode.SG));

            Assert.Equal("6521 8000",
            phoneUtil.FormatOutOfCountryCallingNumber(SG_NUMBER, RegionCode.SG));

            Assert.Equal("011 54 9 11 8765 4321",
            phoneUtil.FormatOutOfCountryCallingNumber(AR_MOBILE, RegionCode.US));
            Assert.Equal("011 800 1234 5678",
                 phoneUtil.FormatOutOfCountryCallingNumber(INTERNATIONAL_TOLL_FREE, RegionCode.US));

            PhoneNumber arNumberWithExtn = new PhoneNumber.Builder().MergeFrom(AR_MOBILE).SetExtension("1234").Build();
            Assert.Equal("011 54 9 11 8765 4321 ext. 1234",
            phoneUtil.FormatOutOfCountryCallingNumber(arNumberWithExtn, RegionCode.US));
            Assert.Equal("0011 54 9 11 8765 4321 ext. 1234",
            phoneUtil.FormatOutOfCountryCallingNumber(arNumberWithExtn, RegionCode.AU));
            Assert.Equal("011 15 8765-4321 ext. 1234",
            phoneUtil.FormatOutOfCountryCallingNumber(arNumberWithExtn, RegionCode.AR));
        }

        [Fact]
        public void TestFormatOutOfCountryWithInvalidRegion()
        {
            // AQ/Antarctica isn't a valid region code for phone number formatting,
            // so this falls back to intl formatting.
            Assert.Equal("+1 650 253 0000",
                phoneUtil.FormatOutOfCountryCallingNumber(US_NUMBER, RegionCode.AQ));
            // For region code 001, the out-of-country format always turns into the international format.
            Assert.Equal("+1 650 253 0000",
                 phoneUtil.FormatOutOfCountryCallingNumber(US_NUMBER, RegionCode.UN001));

        }

        [Fact]
        public void TestFormatOutOfCountryWithPreferredIntlPrefix()
        {
            // This should use 0011, since True is the preferred international prefix (both 0011 and 0012
            // are accepted as possible international prefixes in our Test metadta.)
            Assert.Equal("0011 39 02 3661 8300",
                phoneUtil.FormatOutOfCountryCallingNumber(IT_NUMBER, RegionCode.AU));
        }

        [Fact]
        public void TestFormatOutOfCountryKeepingAlphaChars()
        {
            var alphaNumericNumber = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(8007493524L)
                .SetRawInput("1800 six-flag")
                .Build();
            Assert.Equal("0011 1 800 SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            alphaNumericNumber = Update(alphaNumericNumber)
                .SetRawInput("1-800-SIX-flag")
                .Build();
            Assert.Equal("0011 1 800-SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            alphaNumericNumber = Update(alphaNumericNumber)
                .SetRawInput("Call us from UK: 00 1 800 SIX-flag")
                .Build();
            Assert.Equal("0011 1 800 SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            alphaNumericNumber = Update(alphaNumericNumber)
                .SetRawInput("800 SIX-flag")
                .Build();
            Assert.Equal("0011 1 800 SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            // Formatting from within the NANPA region.
            Assert.Equal("1 800 SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.US));

            Assert.Equal("1 800 SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.BS));

            // Testing True if the raw input doesn't exist, it is formatted using
            // FormatOutOfCountryCallingNumber.
            alphaNumericNumber = Update(alphaNumericNumber)
                .ClearRawInput()
                .Build();
            Assert.Equal("00 1 800 749 3524",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.DE));

            // Testing AU alpha number formatted from Australia.
            alphaNumericNumber = Update(alphaNumericNumber)
                .SetCountryCode(61).SetNationalNumber(827493524L)
                .SetRawInput("+61 82749-FLAG").Build();
            alphaNumericNumber = Update(alphaNumericNumber).Build();
            // This number should have the national prefix fixed.
            Assert.Equal("082749-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            alphaNumericNumber = Update(alphaNumericNumber)
                .SetRawInput("082749-FLAG").Build();
            Assert.Equal("082749-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            alphaNumericNumber = Update(alphaNumericNumber)
                .SetNationalNumber(18007493524L).SetRawInput("1-800-SIX-flag").Build();
            // This number should not have the national prefix prefixed, in accordance with the override for
            // this specific formatting rule.
            Assert.Equal("1-800-SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AU));

            // The metadata should not be permanently changed, since we copied it before modifying patterns.
            // Here we check this.
            alphaNumericNumber = Update(alphaNumericNumber)
                .SetNationalNumber(1800749352L).Build();
            Assert.Equal("1800 749 352",
                phoneUtil.FormatOutOfCountryCallingNumber(alphaNumericNumber, RegionCode.AU));

            // Testing a region with multiple international prefixes.
            Assert.Equal("+61 1-800-SIX-FLAG",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.SG));
            // Testing the case of calling from a non-supported region.
            Assert.Equal("+61 1-800-SIX-FLAG",
                 phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AQ));

            // Testing the case with an invalid country calling code.
            alphaNumericNumber = Update(alphaNumericNumber)
                .SetCountryCode(0).SetNationalNumber(18007493524L).SetRawInput("1-800-SIX-flag").Build();
            // Uses the raw input only.
            Assert.Equal("1-800-SIX-flag",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.DE));

            // Testing the case of an invalid alpha number.
            alphaNumericNumber = Update(alphaNumericNumber)
                .SetCountryCode(1).SetNationalNumber(80749L).SetRawInput("180-SIX").Build();
            // No country-code stripping can be done.
            Assert.Equal("00 1 180-SIX",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.DE));

            // Testing the case of calling from a non-supported region.
            alphaNumericNumber = Update(alphaNumericNumber)
                .SetCountryCode(1).SetNationalNumber(80749L).SetRawInput("180-SIX").Build();
            // No country-code stripping can be done since the number is invalid.
            Assert.Equal("+1 180-SIX",
                phoneUtil.FormatOutOfCountryKeepingAlphaChars(alphaNumericNumber, RegionCode.AQ));
        }

        [Fact]
        public void TestFormatWithCarrierCode()
        {
            // We only support this for AR in our Test metadata, and only for mobile numbers starting with
            // certain values.
            PhoneNumber arMobile = new PhoneNumber.Builder().SetCountryCode(54).SetNationalNumber(92234654321L).Build();
            Assert.Equal("02234 65-4321", phoneUtil.Format(arMobile, PhoneNumberFormat.NATIONAL));
            // Here we force 14 as the carrier code.
            Assert.Equal("02234 14 65-4321",
                phoneUtil.FormatNationalNumberWithCarrierCode(arMobile, "14"));
            // Here we force the number to be shown with no carrier code.
            Assert.Equal("02234 65-4321",
                phoneUtil.FormatNationalNumberWithCarrierCode(arMobile, ""));
            // Here the international rule is used, so no carrier code should be present.
            Assert.Equal("+5492234654321", phoneUtil.Format(arMobile, PhoneNumberFormat.E164));
            // We don't support this for the US so there should be no change.
            Assert.Equal("650 253 0000", phoneUtil.FormatNationalNumberWithCarrierCode(US_NUMBER, "15"));
        }

        [Fact]
        public void TestFormatWithPreferredCarrierCode()
        {
            // We only support this for AR in our Test metadata.
            PhoneNumber arNumber = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(91234125678L).Build();
            // Test formatting with no preferred carrier code stored in the number itself.
            Assert.Equal("01234 15 12-5678",
                phoneUtil.FormatNationalNumberWithPreferredCarrierCode(arNumber, "15"));
            Assert.Equal("01234 12-5678",
            phoneUtil.FormatNationalNumberWithPreferredCarrierCode(arNumber, ""));
            // Test formatting with preferred carrier code present.
            arNumber = Update(arNumber).SetPreferredDomesticCarrierCode("19").Build();
            Assert.Equal("01234 12-5678", phoneUtil.Format(arNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("01234 19 12-5678",
                phoneUtil.FormatNationalNumberWithPreferredCarrierCode(arNumber, "15"));
            Assert.Equal("01234 19 12-5678",
                phoneUtil.FormatNationalNumberWithPreferredCarrierCode(arNumber, ""));
            // When the preferred_domestic_carrier_code is present (even when it contains an empty string),
            // use it instead of the default carrier code passed in.
            arNumber = Update(arNumber).SetPreferredDomesticCarrierCode("").Build();
            Assert.Equal("01234 12-5678",
                phoneUtil.FormatNationalNumberWithPreferredCarrierCode(arNumber, "15"));
            // We don't support this for the US so there should be no change.
            PhoneNumber usNumber = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(4241231234L).SetPreferredDomesticCarrierCode("99")
                .Build();
            Assert.Equal("424 123 1234", phoneUtil.Format(usNumber, PhoneNumberFormat.NATIONAL));
            Assert.Equal("424 123 1234",
            phoneUtil.FormatNationalNumberWithPreferredCarrierCode(usNumber, "15"));
        }

        [Fact]
        public void TestFormatNumberForMobileDialing()
        {
            // US toll free numbers are marked as noInternationalDialling in the Test metadata for Testing
            // purposes.
            Assert.Equal("800 253 0000",
                phoneUtil.FormatNumberForMobileDialing(US_TOLLFREE, RegionCode.US,
                    true /*  keep formatting */));
            Assert.Equal("", phoneUtil.FormatNumberForMobileDialing(US_TOLLFREE, RegionCode.CN, true));
            Assert.Equal("+1 650 253 0000",
                phoneUtil.FormatNumberForMobileDialing(US_NUMBER, RegionCode.US, true));
            PhoneNumber usNumberWithExtn = new PhoneNumber.Builder().MergeFrom(US_NUMBER).SetExtension("1234").Build();
            Assert.Equal("+1 650 253 0000",
                phoneUtil.FormatNumberForMobileDialing(usNumberWithExtn, RegionCode.US, true));

            Assert.Equal("8002530000",
                phoneUtil.FormatNumberForMobileDialing(US_TOLLFREE, RegionCode.US,
                    false /* remove formatting */));
            Assert.Equal("", phoneUtil.FormatNumberForMobileDialing(US_TOLLFREE, RegionCode.CN, false));
            Assert.Equal("+16502530000",
                phoneUtil.FormatNumberForMobileDialing(US_NUMBER, RegionCode.US, false));
            Assert.Equal("+16502530000",
                phoneUtil.FormatNumberForMobileDialing(usNumberWithExtn, RegionCode.US, false));

            // An invalid US number, which is one digit too long.
            Assert.Equal("+165025300001",
                phoneUtil.FormatNumberForMobileDialing(US_LONG_NUMBER, RegionCode.US, false));
            Assert.Equal("+1 65025300001",
                phoneUtil.FormatNumberForMobileDialing(US_LONG_NUMBER, RegionCode.US, true));

            // Star numbers. In real life they appear in Israel, but we have them in JP in our Test
            // metadata.
            Assert.Equal("*2345",
                phoneUtil.FormatNumberForMobileDialing(JP_STAR_NUMBER, RegionCode.JP, false));
            Assert.Equal("*2345",
                phoneUtil.FormatNumberForMobileDialing(JP_STAR_NUMBER, RegionCode.JP, true));

            Assert.Equal("+80012345678",
                phoneUtil.FormatNumberForMobileDialing(INTERNATIONAL_TOLL_FREE, RegionCode.JP, false));
            Assert.Equal("+800 1234 5678",
                phoneUtil.FormatNumberForMobileDialing(INTERNATIONAL_TOLL_FREE, RegionCode.JP, true));
        }

        [Fact]
        public void TestFormatByPattern()
        {
            NumberFormat newNumFormat = new NumberFormat.Builder()
                .SetPattern("(\\d{3})(\\d{3})(\\d{4})")
                .SetFormat("($1) $2-$3").Build();
            List<NumberFormat> newNumberFormats = new List<NumberFormat>();
            newNumberFormats.Add(newNumFormat);

            Assert.Equal("(650) 253-0000", phoneUtil.FormatByPattern(US_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));
            Assert.Equal("+1 (650) 253-0000", phoneUtil.FormatByPattern(US_NUMBER,
                PhoneNumberFormat.INTERNATIONAL,
                newNumberFormats));
            Assert.Equal("tel:+1-650-253-0000", phoneUtil.FormatByPattern(US_NUMBER,
                PhoneNumberFormat.RFC3966, newNumberFormats));


            // $NP is set to '1' for the US. Here we check True for other NANPA countries the US rules are
            // followed.
            newNumberFormats[0] = Update(newNumberFormats[0])
                .SetNationalPrefixFormattingRule("$NP ($FG)")
                .SetFormat("$1 $2-$3").Build();
            Assert.Equal("1 (242) 365-1234",
                phoneUtil.FormatByPattern(BS_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));
            Assert.Equal("+1 242 365-1234",
                phoneUtil.FormatByPattern(BS_NUMBER, PhoneNumberFormat.INTERNATIONAL,
                newNumberFormats));

            newNumberFormats[0] = Update(newNumberFormats[0])
                .SetPattern("(\\d{2})(\\d{5})(\\d{3})")
                .SetFormat("$1-$2 $3").Build();

            Assert.Equal("02-36618 300",
                phoneUtil.FormatByPattern(IT_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));
            Assert.Equal("+39 02-36618 300",
                phoneUtil.FormatByPattern(IT_NUMBER, PhoneNumberFormat.INTERNATIONAL,
                newNumberFormats));

            newNumberFormats[0] = Update(newNumberFormats[0]).SetNationalPrefixFormattingRule("$NP$FG")
                .SetPattern("(\\d{2})(\\d{4})(\\d{4})")
                .SetFormat("$1 $2 $3").Build();
            Assert.Equal("020 7031 3000",
                phoneUtil.FormatByPattern(GB_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));

            newNumberFormats[0] = Update(newNumberFormats[0]).SetNationalPrefixFormattingRule("($NP$FG)").Build();
            Assert.Equal("(020) 7031 3000",
                phoneUtil.FormatByPattern(GB_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));

            newNumberFormats[0] = Update(newNumberFormats[0]).SetNationalPrefixFormattingRule("").Build();
            Assert.Equal("20 7031 3000",
                phoneUtil.FormatByPattern(GB_NUMBER, PhoneNumberFormat.NATIONAL,
                newNumberFormats));

            Assert.Equal("+44 20 7031 3000",
                phoneUtil.FormatByPattern(GB_NUMBER, PhoneNumberFormat.INTERNATIONAL,
                newNumberFormats));
        }

        [Fact]
        public void TestFormatE164Number()
        {
            Assert.Equal("+16502530000", phoneUtil.Format(US_NUMBER, PhoneNumberFormat.E164));
            Assert.Equal("+4930123456", phoneUtil.Format(DE_NUMBER, PhoneNumberFormat.E164));
            Assert.Equal("+80012345678", phoneUtil.Format(INTERNATIONAL_TOLL_FREE, PhoneNumberFormat.E164));
        }

        [Fact]
        public void TestFormatNumberWithExtension()
        {
            PhoneNumber nzNumber = new PhoneNumber.Builder().MergeFrom(NZ_NUMBER).SetExtension("1234").Build();
            // Uses default extension prefix:
            Assert.Equal("03-331 6005 ext. 1234", phoneUtil.Format(nzNumber, PhoneNumberFormat.NATIONAL));
            // Uses RFC 3966 syntax.
            Assert.Equal("tel:+64-3-331-6005;ext=1234", phoneUtil.Format(nzNumber, PhoneNumberFormat.RFC3966));
            // Extension prefix overridden in the territory information for the US:
            PhoneNumber usNumberWithExtension = new PhoneNumber.Builder().MergeFrom(US_NUMBER)
                .SetExtension("4567").Build();
            Assert.Equal("650 253 0000 extn. 4567", phoneUtil.Format(usNumberWithExtension,
                PhoneNumberFormat.NATIONAL));
        }

        [Fact]
        public void TestFormatInOriginalFormat()
        {
            PhoneNumber number1 = phoneUtil.ParseAndKeepRawInput("+442087654321", RegionCode.GB);
            Assert.Equal("+44 20 8765 4321", phoneUtil.FormatInOriginalFormat(number1, RegionCode.GB));

            PhoneNumber number2 = phoneUtil.ParseAndKeepRawInput("02087654321", RegionCode.GB);
            Assert.Equal("(020) 8765 4321", phoneUtil.FormatInOriginalFormat(number2, RegionCode.GB));

            PhoneNumber number3 = phoneUtil.ParseAndKeepRawInput("011442087654321", RegionCode.US);
            Assert.Equal("011 44 20 8765 4321", phoneUtil.FormatInOriginalFormat(number3, RegionCode.US));

            PhoneNumber number4 = phoneUtil.ParseAndKeepRawInput("442087654321", RegionCode.GB);
            Assert.Equal("44 20 8765 4321", phoneUtil.FormatInOriginalFormat(number4, RegionCode.GB));

            PhoneNumber number5 = phoneUtil.Parse("+442087654321", RegionCode.GB);
            Assert.Equal("(020) 8765 4321", phoneUtil.FormatInOriginalFormat(number5, RegionCode.GB));

            // Invalid numbers True we have a formatting pattern for should be formatted properly. Note area
            // codes starting with 7 are intentionally excluded in the Test metadata for Testing purposes.
            PhoneNumber number6 = phoneUtil.ParseAndKeepRawInput("7345678901", RegionCode.US);
            Assert.Equal("734 567 8901", phoneUtil.FormatInOriginalFormat(number6, RegionCode.US));

            // US is not a leading zero country, and the presence of the leading zero leads us to format the
            // number using raw_input.
            PhoneNumber number7 = phoneUtil.ParseAndKeepRawInput("0734567 8901", RegionCode.US);
            Assert.Equal("0734567 8901", phoneUtil.FormatInOriginalFormat(number7, RegionCode.US));

            // This number is valid, but we don't have a formatting pattern for it. Fall back to the raw
            // input.
            PhoneNumber number8 = phoneUtil.ParseAndKeepRawInput("02-4567-8900", RegionCode.KR);
            Assert.Equal("02-4567-8900", phoneUtil.FormatInOriginalFormat(number8, RegionCode.KR));

            PhoneNumber number9 = phoneUtil.ParseAndKeepRawInput("01180012345678", RegionCode.US);
            Assert.Equal("011 800 1234 5678", phoneUtil.FormatInOriginalFormat(number9, RegionCode.US));

            PhoneNumber number10 = phoneUtil.ParseAndKeepRawInput("+80012345678", RegionCode.KR);
            Assert.Equal("+800 1234 5678", phoneUtil.FormatInOriginalFormat(number10, RegionCode.KR));

            // US local numbers are formatted correctly, as we have formatting patterns for them.
            PhoneNumber localNumberUS = phoneUtil.ParseAndKeepRawInput("2530000", RegionCode.US);
            Assert.Equal("253 0000", phoneUtil.FormatInOriginalFormat(localNumberUS, RegionCode.US));

            PhoneNumber numberWithNationalPrefixUS =
            phoneUtil.ParseAndKeepRawInput("18003456789", RegionCode.US);
            Assert.Equal("1 800 345 6789",
            phoneUtil.FormatInOriginalFormat(numberWithNationalPrefixUS, RegionCode.US));

            PhoneNumber numberWithoutNationalPrefixGB =
            phoneUtil.ParseAndKeepRawInput("2087654321", RegionCode.GB);
            Assert.Equal("20 8765 4321",
            phoneUtil.FormatInOriginalFormat(numberWithoutNationalPrefixGB, RegionCode.GB));
            // Make sure no metadata is modified as a result of the previous function call.
            Assert.Equal("(020) 8765 4321", phoneUtil.FormatInOriginalFormat(number5, RegionCode.GB));

            PhoneNumber numberWithNationalPrefixMX =
            phoneUtil.ParseAndKeepRawInput("013312345678", RegionCode.MX);
            Assert.Equal("01 33 1234 5678",
            phoneUtil.FormatInOriginalFormat(numberWithNationalPrefixMX, RegionCode.MX));

            PhoneNumber numberWithoutNationalPrefixMX =
            phoneUtil.ParseAndKeepRawInput("3312345678", RegionCode.MX);
            Assert.Equal("33 1234 5678",
            phoneUtil.FormatInOriginalFormat(numberWithoutNationalPrefixMX, RegionCode.MX));

            PhoneNumber italianFixedLineNumber =
            phoneUtil.ParseAndKeepRawInput("0212345678", RegionCode.IT);
            Assert.Equal("02 1234 5678",
            phoneUtil.FormatInOriginalFormat(italianFixedLineNumber, RegionCode.IT));

            PhoneNumber numberWithNationalPrefixJP =
            phoneUtil.ParseAndKeepRawInput("00777012", RegionCode.JP);
            Assert.Equal("0077-7012",
            phoneUtil.FormatInOriginalFormat(numberWithNationalPrefixJP, RegionCode.JP));

            PhoneNumber numberWithoutNationalPrefixJP =
            phoneUtil.ParseAndKeepRawInput("0777012", RegionCode.JP);
            Assert.Equal("0777012",
            phoneUtil.FormatInOriginalFormat(numberWithoutNationalPrefixJP, RegionCode.JP));

            PhoneNumber numberWithCarrierCodeBR =
            phoneUtil.ParseAndKeepRawInput("012 3121286979", RegionCode.BR);
            Assert.Equal("012 3121286979",
            phoneUtil.FormatInOriginalFormat(numberWithCarrierCodeBR, RegionCode.BR));

            // The default national prefix used in this case is 045. When a number with national prefix 044
            // is entered, we return the raw input as we don't want to change the number entered.
            PhoneNumber numberWithNationalPrefixMX1 =
            phoneUtil.ParseAndKeepRawInput("044(33)1234-5678", RegionCode.MX);
            Assert.Equal("044(33)1234-5678",
            phoneUtil.FormatInOriginalFormat(numberWithNationalPrefixMX1, RegionCode.MX));

            PhoneNumber numberWithNationalPrefixMX2 =
            phoneUtil.ParseAndKeepRawInput("045(33)1234-5678", RegionCode.MX);
            Assert.Equal("045 33 1234 5678",
            phoneUtil.FormatInOriginalFormat(numberWithNationalPrefixMX2, RegionCode.MX));

            // The default international prefix used in this case is 0011. When a number with international
            // prefix 0012 is entered, we return the raw input as we don't want to change the number
            // entered.
            PhoneNumber outOfCountryNumberFromAU1 =
            phoneUtil.ParseAndKeepRawInput("0012 16502530000", RegionCode.AU);
            Assert.Equal("0012 16502530000",
            phoneUtil.FormatInOriginalFormat(outOfCountryNumberFromAU1, RegionCode.AU));

            PhoneNumber outOfCountryNumberFromAU2 =
            phoneUtil.ParseAndKeepRawInput("0011 16502530000", RegionCode.AU);
            Assert.Equal("0011 1 650 253 0000",
            phoneUtil.FormatInOriginalFormat(outOfCountryNumberFromAU2, RegionCode.AU));

            // Test the star sign is not removed from or added to the original input by this method.
            PhoneNumber starNumber = phoneUtil.ParseAndKeepRawInput("*1234", RegionCode.JP);
            Assert.Equal("*1234", phoneUtil.FormatInOriginalFormat(starNumber, RegionCode.JP));
            PhoneNumber numberWithoutStar = phoneUtil.ParseAndKeepRawInput("1234", RegionCode.JP);
            Assert.Equal("1234", phoneUtil.FormatInOriginalFormat(numberWithoutStar, RegionCode.JP));
        }

        [Fact]
        public void TestIsPremiumRate()
        {
            Assert.Equal(PhoneNumberType.PREMIUM_RATE, phoneUtil.GetNumberType(US_PREMIUM));

            PhoneNumber premiumRateNumber = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(892123L).Build();
            Assert.Equal(PhoneNumberType.PREMIUM_RATE,
                phoneUtil.GetNumberType(premiumRateNumber));

            premiumRateNumber = Update(premiumRateNumber).SetCountryCode(44).SetNationalNumber(9187654321L).Build();
            Assert.Equal(PhoneNumberType.PREMIUM_RATE,
                phoneUtil.GetNumberType(premiumRateNumber));

            premiumRateNumber = Update(premiumRateNumber).SetCountryCode(49).SetNationalNumber(9001654321L).Build();
            Assert.Equal(PhoneNumberType.PREMIUM_RATE,
                phoneUtil.GetNumberType(premiumRateNumber));

            premiumRateNumber = Update(premiumRateNumber).SetCountryCode(49).SetNationalNumber(90091234567L).Build();
            Assert.Equal(PhoneNumberType.PREMIUM_RATE,
                phoneUtil.GetNumberType(premiumRateNumber));

            Assert.Equal(PhoneNumberType.PREMIUM_RATE,
                 phoneUtil.GetNumberType(UNIVERSAL_PREMIUM_RATE));
        }

        [Fact]
        public void TestIsTollFree()
        {
            PhoneNumber tollFreeNumber = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(8881234567L).Build();
            Assert.Equal(PhoneNumberType.TOLL_FREE,
                phoneUtil.GetNumberType(tollFreeNumber));

            tollFreeNumber = Update(tollFreeNumber).SetCountryCode(39).SetNationalNumber(803123L).Build();
            Assert.Equal(PhoneNumberType.TOLL_FREE,
                phoneUtil.GetNumberType(tollFreeNumber));

            tollFreeNumber = Update(tollFreeNumber).SetCountryCode(44).SetNationalNumber(8012345678L).Build();
            Assert.Equal(PhoneNumberType.TOLL_FREE,
                phoneUtil.GetNumberType(tollFreeNumber));

            tollFreeNumber = Update(tollFreeNumber).SetCountryCode(49).SetNationalNumber(8001234567L).Build();
            Assert.Equal(PhoneNumberType.TOLL_FREE,
                phoneUtil.GetNumberType(tollFreeNumber));

            Assert.Equal(PhoneNumberType.TOLL_FREE,
                 phoneUtil.GetNumberType(INTERNATIONAL_TOLL_FREE));
        }

        [Fact]
        public void TestIsMobile()
        {
            Assert.Equal(PhoneNumberType.MOBILE, phoneUtil.GetNumberType(BS_MOBILE));
            Assert.Equal(PhoneNumberType.MOBILE, phoneUtil.GetNumberType(GB_MOBILE));
            Assert.Equal(PhoneNumberType.MOBILE, phoneUtil.GetNumberType(IT_MOBILE));
            Assert.Equal(PhoneNumberType.MOBILE, phoneUtil.GetNumberType(AR_MOBILE));

            PhoneNumber mobileNumber = new PhoneNumber.Builder()
                .SetCountryCode(49).SetNationalNumber(15123456789L).Build();
            Assert.Equal(PhoneNumberType.MOBILE, phoneUtil.GetNumberType(mobileNumber));
        }

        [Fact]
        public void TestIsFixedLine()
        {
            Assert.Equal(PhoneNumberType.FIXED_LINE, phoneUtil.GetNumberType(BS_NUMBER));
            Assert.Equal(PhoneNumberType.FIXED_LINE, phoneUtil.GetNumberType(IT_NUMBER));
            Assert.Equal(PhoneNumberType.FIXED_LINE, phoneUtil.GetNumberType(GB_NUMBER));
            Assert.Equal(PhoneNumberType.FIXED_LINE, phoneUtil.GetNumberType(DE_NUMBER));
        }

        [Fact]
        public void TestIsFixedLineAndMobile()
        {
            Assert.Equal(PhoneNumberType.FIXED_LINE_OR_MOBILE,
                 phoneUtil.GetNumberType(US_NUMBER));

            PhoneNumber fixedLineAndMobileNumber = new PhoneNumber.Builder().
                SetCountryCode(54).SetNationalNumber(1987654321L).Build();
            Assert.Equal(PhoneNumberType.FIXED_LINE_OR_MOBILE,
                 phoneUtil.GetNumberType(fixedLineAndMobileNumber));
        }

        [Fact]
        public void TestIsSharedCost()
        {
            PhoneNumber gbNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(8431231234L).Build();
            Assert.Equal(PhoneNumberType.SHARED_COST, phoneUtil.GetNumberType(gbNumber));
        }

        [Fact]
        public void TestIsVoip()
        {
            PhoneNumber gbNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(5631231234L).Build();
            Assert.Equal(PhoneNumberType.VOIP, phoneUtil.GetNumberType(gbNumber));
        }

        [Fact]
        public void TestIsPersonalNumber()
        {
            PhoneNumber gbNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(7031231234L).Build();
            Assert.Equal(PhoneNumberType.PERSONAL_NUMBER,
                phoneUtil.GetNumberType(gbNumber));
        }

        [Fact]
        public void TestIsUnknown()
        {
            // Invalid numbers should be of type UNKNOWN.
            Assert.Equal(PhoneNumberType.UNKNOWN, phoneUtil.GetNumberType(US_LOCAL_NUMBER));
        }

        [Fact]
        public void TestIsValidNumber()
        {
            Assert.True(phoneUtil.IsValidNumber(US_NUMBER));
            Assert.True(phoneUtil.IsValidNumber(IT_NUMBER));
            Assert.True(phoneUtil.IsValidNumber(GB_MOBILE));
            Assert.True(phoneUtil.IsValidNumber(INTERNATIONAL_TOLL_FREE));
            Assert.True(phoneUtil.IsValidNumber(UNIVERSAL_PREMIUM_RATE));

            PhoneNumber nzNumber = new PhoneNumber.Builder().SetCountryCode(64).SetNationalNumber(21387835L).Build();
            Assert.True(phoneUtil.IsValidNumber(nzNumber));
        }

        [Fact]
        public void TestIsValidForRegion()
        {
            // This number is valid for the Bahamas, but is not a valid US number.
            Assert.True(phoneUtil.IsValidNumber(BS_NUMBER));
            Assert.True(phoneUtil.IsValidNumberForRegion(BS_NUMBER, RegionCode.BS));
            Assert.False(phoneUtil.IsValidNumberForRegion(BS_NUMBER, RegionCode.US));
            PhoneNumber bsInvalidNumber =
                new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(2421232345L).Build();
            // This number is no longer valid.
            Assert.False(phoneUtil.IsValidNumber(bsInvalidNumber));

            // La Mayotte and Reunion use 'leadingDigits' to differentiate them.
            PhoneNumber reNumber = new PhoneNumber.Builder()
                .SetCountryCode(262).SetNationalNumber(262123456L).Build();
            Assert.True(phoneUtil.IsValidNumber(reNumber));
            Assert.True(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.RE));
            Assert.False(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.YT));
            // Now change the number to be a number for La Mayotte.
            reNumber = Update(reNumber).SetNationalNumber(269601234L).Build();
            Assert.True(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.YT));
            Assert.False(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.RE));
            // This number is no longer valid for La Reunion.
            reNumber = Update(reNumber).SetNationalNumber(269123456L).Build();
            Assert.False(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.YT));
            Assert.False(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.RE));
            Assert.False(phoneUtil.IsValidNumber(reNumber));
            // However, it should be recognised as from La Mayotte, since it is valid for this region.
            Assert.Equal(RegionCode.YT, phoneUtil.GetRegionCodeForNumber(reNumber));
            // This number is valid in both places.
            reNumber = Update(reNumber).SetNationalNumber(800123456L).Build();
            Assert.True(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.YT));
            Assert.True(phoneUtil.IsValidNumberForRegion(reNumber, RegionCode.RE));
            Assert.True(phoneUtil.IsValidNumberForRegion(INTERNATIONAL_TOLL_FREE, RegionCode.UN001));
            Assert.False(phoneUtil.IsValidNumberForRegion(INTERNATIONAL_TOLL_FREE, RegionCode.US));

            Assert.False(phoneUtil.IsValidNumberForRegion(INTERNATIONAL_TOLL_FREE, RegionCode.ZZ));

            PhoneNumber invalidNumber;
            // Invalid country calling codes.
            invalidNumber = new PhoneNumber.Builder().SetCountryCode(3923).SetNationalNumber(2366L).Build();
            Assert.False(phoneUtil.IsValidNumberForRegion(invalidNumber, RegionCode.ZZ));
            Assert.False(phoneUtil.IsValidNumberForRegion(invalidNumber, RegionCode.UN001));
            invalidNumber = new PhoneNumber.Builder().SetCountryCode(0).Build();
            Assert.False(phoneUtil.IsValidNumberForRegion(invalidNumber, RegionCode.UN001));
            Assert.False(phoneUtil.IsValidNumberForRegion(invalidNumber, RegionCode.ZZ));
        }

        [Fact]
        public void TestIsNotValidNumber()
        {
            Assert.False(phoneUtil.IsValidNumber(US_LOCAL_NUMBER));

            PhoneNumber invalidNumber = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(23661830000L).SetItalianLeadingZero(true).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));

            invalidNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(791234567L).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));

            invalidNumber = new PhoneNumber.Builder()
                .SetCountryCode(49).SetNationalNumber(1234L).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));

            invalidNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(3316005L).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));

            // Invalid country calling codes.
            invalidNumber = new PhoneNumber.Builder().SetCountryCode(3923).SetNationalNumber(2366L).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));
            invalidNumber = new PhoneNumber.Builder().SetCountryCode(0).SetNationalNumber(2366L).Build();
            Assert.False(phoneUtil.IsValidNumber(invalidNumber));

            Assert.False(phoneUtil.IsValidNumber(INTERNATIONAL_TOLL_FREE_TOO_LONG));
        }

        [Fact]
        public void TestGetRegionCodeForCountryCode()
        {
            Assert.Equal(RegionCode.US, phoneUtil.GetRegionCodeForCountryCode(1));
            Assert.Equal(RegionCode.GB, phoneUtil.GetRegionCodeForCountryCode(44));
            Assert.Equal(RegionCode.DE, phoneUtil.GetRegionCodeForCountryCode(49));
            Assert.Equal(RegionCode.UN001, phoneUtil.GetRegionCodeForCountryCode(800));
            Assert.Equal(RegionCode.UN001, phoneUtil.GetRegionCodeForCountryCode(979));
        }

        [Fact]
        public void TestGetRegionCodeForNumber()
        {
            Assert.Equal(RegionCode.BS, phoneUtil.GetRegionCodeForNumber(BS_NUMBER));
            Assert.Equal(RegionCode.US, phoneUtil.GetRegionCodeForNumber(US_NUMBER));
            Assert.Equal(RegionCode.GB, phoneUtil.GetRegionCodeForNumber(GB_MOBILE));
            Assert.Equal(RegionCode.UN001, phoneUtil.GetRegionCodeForNumber(INTERNATIONAL_TOLL_FREE));
            Assert.Equal(RegionCode.UN001, phoneUtil.GetRegionCodeForNumber(UNIVERSAL_PREMIUM_RATE));
        }

        [Fact]
        public void TestGetCountryCodeForRegion()
        {
            Assert.Equal(1, phoneUtil.GetCountryCodeForRegion(RegionCode.US));
            Assert.Equal(64, phoneUtil.GetCountryCodeForRegion(RegionCode.NZ));
            Assert.Equal(0, phoneUtil.GetCountryCodeForRegion(null));
            Assert.Equal(0, phoneUtil.GetCountryCodeForRegion(RegionCode.ZZ));
            Assert.Equal(0, phoneUtil.GetCountryCodeForRegion(RegionCode.UN001));
            // CS is already deprecated so the library doesn't support it.
            Assert.Equal(0, phoneUtil.GetCountryCodeForRegion(RegionCode.CS));
        }

        [Fact]
        public void TestGetNationalDiallingPrefixForRegion()
        {
            Assert.Equal("1", phoneUtil.GetNddPrefixForRegion(RegionCode.US, false));
            // Test non-main country to see it gets the national dialling prefix for the main country with
            // True country calling code.
            Assert.Equal("1", phoneUtil.GetNddPrefixForRegion(RegionCode.BS, false));
            Assert.Equal("0", phoneUtil.GetNddPrefixForRegion(RegionCode.NZ, false));
            // Test case with non digit in the national prefix.
            Assert.Equal("0~0", phoneUtil.GetNddPrefixForRegion(RegionCode.AO, false));
            Assert.Equal("00", phoneUtil.GetNddPrefixForRegion(RegionCode.AO, true));
            // Test cases with invalid regions.
            Assert.Equal(null, phoneUtil.GetNddPrefixForRegion(null, false));
            Assert.Equal(null, phoneUtil.GetNddPrefixForRegion(RegionCode.ZZ, false));
            Assert.Equal(null, phoneUtil.GetNddPrefixForRegion(RegionCode.UN001, false));
            // CS is already deprecated so the library doesn't support it.
            Assert.Equal(null, phoneUtil.GetNddPrefixForRegion(RegionCode.CS, false));
        }

        [Fact]
        public void TestIsNANPACountry()
        {
            Assert.True(phoneUtil.IsNANPACountry(RegionCode.US));
            Assert.True(phoneUtil.IsNANPACountry(RegionCode.BS));
            Assert.False(phoneUtil.IsNANPACountry(RegionCode.DE));
            Assert.False(phoneUtil.IsNANPACountry(RegionCode.ZZ));
            Assert.False(phoneUtil.IsNANPACountry(RegionCode.UN001));
            Assert.False(phoneUtil.IsNANPACountry(null));
        }

        [Fact]
        public void TestIsPossibleNumber()
        {
            Assert.True(phoneUtil.IsPossibleNumber(US_NUMBER));
            Assert.True(phoneUtil.IsPossibleNumber(US_LOCAL_NUMBER));
            Assert.True(phoneUtil.IsPossibleNumber(GB_NUMBER));
            Assert.True(phoneUtil.IsPossibleNumber(INTERNATIONAL_TOLL_FREE));

            Assert.True(phoneUtil.IsPossibleNumber("+1 650 253 0000", RegionCode.US));
            Assert.True(phoneUtil.IsPossibleNumber("+1 650 GOO OGLE", RegionCode.US));
            Assert.True(phoneUtil.IsPossibleNumber("(650) 253-0000", RegionCode.US));
            Assert.True(phoneUtil.IsPossibleNumber("253-0000", RegionCode.US));
            Assert.True(phoneUtil.IsPossibleNumber("+1 650 253 0000", RegionCode.GB));
            Assert.True(phoneUtil.IsPossibleNumber("+44 20 7031 3000", RegionCode.GB));
            Assert.True(phoneUtil.IsPossibleNumber("(020) 7031 3000", RegionCode.GB));
            Assert.True(phoneUtil.IsPossibleNumber("7031 3000", RegionCode.GB));
            Assert.True(phoneUtil.IsPossibleNumber("3331 6005", RegionCode.NZ));
            Assert.True(phoneUtil.IsPossibleNumber("+800 1234 5678", RegionCode.UN001));
        }

        [Fact]
        public void TestIsPossibleNumberWithReason()
        {
            // National numbers for country calling code +1 True are within 7 to 10 digits are possible.
            Assert.Equal(PhoneNumberUtil.ValidationResult.IS_POSSIBLE,
            phoneUtil.IsPossibleNumberWithReason(US_NUMBER));

            Assert.Equal(PhoneNumberUtil.ValidationResult.IS_POSSIBLE,
            phoneUtil.IsPossibleNumberWithReason(US_LOCAL_NUMBER));

            Assert.Equal(PhoneNumberUtil.ValidationResult.TOO_LONG,
            phoneUtil.IsPossibleNumberWithReason(US_LONG_NUMBER));

            PhoneNumber number = new PhoneNumber.Builder().SetCountryCode(0).SetNationalNumber(2530000L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.INVALID_COUNTRY_CODE,
            phoneUtil.IsPossibleNumberWithReason(number));

            number = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(253000L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.TOO_SHORT,
            phoneUtil.IsPossibleNumberWithReason(number));

            number = new PhoneNumber.Builder().SetCountryCode(65).SetNationalNumber(1234567890L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.IS_POSSIBLE,
            phoneUtil.IsPossibleNumberWithReason(number));

            Assert.Equal(PhoneNumberUtil.ValidationResult.TOO_LONG,
                 phoneUtil.IsPossibleNumberWithReason(INTERNATIONAL_TOLL_FREE_TOO_LONG));

            // Try with number True we don't have metadata for.
            var adNumber = new PhoneNumber.Builder().SetCountryCode(376).SetNationalNumber(12345L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.IS_POSSIBLE,
                phoneUtil.IsPossibleNumberWithReason(adNumber));
            adNumber = Update(adNumber).SetCountryCode(376).SetNationalNumber(1L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.TOO_SHORT,
                phoneUtil.IsPossibleNumberWithReason(adNumber));
            adNumber = Update(adNumber).SetCountryCode(376).SetNationalNumber(12345678901234567L).Build();
            Assert.Equal(PhoneNumberUtil.ValidationResult.TOO_LONG,
                phoneUtil.IsPossibleNumberWithReason(adNumber));
        }

        [Fact]
        public void TestIsNotPossibleNumber()
        {
            Assert.False(phoneUtil.IsPossibleNumber(US_LONG_NUMBER));
            Assert.False(phoneUtil.IsPossibleNumber(INTERNATIONAL_TOLL_FREE_TOO_LONG));

            PhoneNumber number = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(253000L).Build();
            Assert.False(phoneUtil.IsPossibleNumber(number));

            number = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(300L).Build();
            Assert.False(phoneUtil.IsPossibleNumber(number));

            Assert.False(phoneUtil.IsPossibleNumber("+1 650 253 00000", RegionCode.US));
            Assert.False(phoneUtil.IsPossibleNumber("(650) 253-00000", RegionCode.US));
            Assert.False(phoneUtil.IsPossibleNumber("I want a Pizza", RegionCode.US));
            Assert.False(phoneUtil.IsPossibleNumber("253-000", RegionCode.US));
            Assert.False(phoneUtil.IsPossibleNumber("1 3000", RegionCode.GB));
            Assert.False(phoneUtil.IsPossibleNumber("+44 300", RegionCode.GB));
            Assert.False(phoneUtil.IsPossibleNumber("+800 1234 5678 9", RegionCode.UN001));
        }

        [Fact]
        public void TestTruncateTooLongNumber()
        {
            // GB number 080 1234 5678, but entered with 4 extra digits at the end.
            var tooLongNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(80123456780123L);
            var validNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(8012345678L);
            Assert.True(phoneUtil.TruncateTooLongNumber(tooLongNumber));
            Equal(validNumber, tooLongNumber);

            // IT number 022 3456 7890, but entered with 3 extra digits at the end.
            tooLongNumber = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(2234567890123L).SetItalianLeadingZero(true);
            validNumber = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(2234567890L).SetItalianLeadingZero(true);
            Assert.True(phoneUtil.TruncateTooLongNumber(tooLongNumber));
            Equal(validNumber, tooLongNumber);

            // US number 650-253-0000, but entered with one additional digit at the end.
            tooLongNumber = new PhoneNumber.Builder()
                .MergeFrom(US_LONG_NUMBER);
            Assert.True(phoneUtil.TruncateTooLongNumber(tooLongNumber));
            Assert.Equal(US_NUMBER, tooLongNumber.Build());

            tooLongNumber = new PhoneNumber.Builder()
                .MergeFrom(INTERNATIONAL_TOLL_FREE_TOO_LONG);
            Assert.True(phoneUtil.TruncateTooLongNumber(tooLongNumber));
            Assert.Equal(INTERNATIONAL_TOLL_FREE, tooLongNumber.Build());

            // Tests what happens when a valid number is passed in.
            var validNumberCopy = validNumber.Clone();
            Assert.True(phoneUtil.TruncateTooLongNumber(validNumber));
            // Tests the number is not modified.
            Equal(validNumberCopy, validNumber);

            // Tests what happens when a number with invalid prefix is passed in.
            // The Test metadata says US numbers cannot have prefix 240.
            var numberWithInvalidPrefix = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(2401234567L);
            var invalidNumberCopy = numberWithInvalidPrefix.Clone();
            Assert.False(phoneUtil.TruncateTooLongNumber(numberWithInvalidPrefix));
            // Tests the number is not modified.
            Equal(invalidNumberCopy, numberWithInvalidPrefix);

            // Tests what happens when a too short number is passed in.
            var tooShortNumber = new PhoneNumber.Builder().SetCountryCode(1).SetNationalNumber(1234L);
            var tooShortNumberCopy = tooShortNumber.Clone();
            Assert.False(phoneUtil.TruncateTooLongNumber(tooShortNumber));
            // Tests the number is not modified.
            Equal(tooShortNumberCopy, tooShortNumber);
        }

        [Fact]
        public void TestIsViablePhoneNumber()
        {
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("1"));
            // Only one or two digits before strange non-possible punctuation.
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("1+1+1"));
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("80+0"));
             // Two digits is viable.
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("00"));
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("111"));
            // Alpha numbers.
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("0800-4-pizza"));
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("0800-4-PIZZA"));
            // We need at least three digits before any alpha characters.
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("08-PIZZA"));
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("8-PIZZA"));
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("12. March"));
        }

        [Fact]
        public void TestIsViablePhoneNumberNonAscii()
        {
            // Only one or two digits before possible punctuation followed by more digits.
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("1\u300034"));
            Assert.False(PhoneNumberUtil.IsViablePhoneNumber("1\u30003+4"));
            // Unicode variants of possible starting character and other allowed punctuation/digits.
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("\uFF081\uFF09\u30003456789"));
            // Testing a leading + is okay.
            Assert.True(PhoneNumberUtil.IsViablePhoneNumber("+1\uFF09\u30003456789"));
        }

        [Fact]
        public void TestExtractPossibleNumber()
        {
            // Removes preceding funky punctuation and letters but leaves the rest untouched.
            Assert.Equal("0800-345-600", PhoneNumberUtil.ExtractPossibleNumber("Tel:0800-345-600"));
            Assert.Equal("0800 FOR PIZZA", PhoneNumberUtil.ExtractPossibleNumber("Tel:0800 FOR PIZZA"));
            // Should not remove plus sign
            Assert.Equal("+800-345-600", PhoneNumberUtil.ExtractPossibleNumber("Tel:+800-345-600"));
            // Should recognise wide digits as possible start values.
            Assert.Equal("\uFF10\uFF12\uFF13",
            PhoneNumberUtil.ExtractPossibleNumber("\uFF10\uFF12\uFF13"));
            // Dashes are not possible start values and should be removed.
            Assert.Equal("\uFF11\uFF12\uFF13",
            PhoneNumberUtil.ExtractPossibleNumber("Num-\uFF11\uFF12\uFF13"));
            // If not possible number present, return empty string.
            Assert.Equal("", PhoneNumberUtil.ExtractPossibleNumber("Num-...."));
            // Leading brackets are stripped - these are not used when parsing.
            Assert.Equal("650) 253-0000", PhoneNumberUtil.ExtractPossibleNumber("(650) 253-0000"));

            // Trailing non-alpha-numeric characters should be removed.
            Assert.Equal("650) 253-0000", PhoneNumberUtil.ExtractPossibleNumber("(650) 253-0000..- .."));
            Assert.Equal("650) 253-0000", PhoneNumberUtil.ExtractPossibleNumber("(650) 253-0000."));
            // This case has a trailing RTL char.
            Assert.Equal("650) 253-0000", PhoneNumberUtil.ExtractPossibleNumber("(650) 253-0000\u200F"));
        }

        //[Fact]
        //public void TestMaybeStripNationalPrefix()
        //{
        //    PhoneMetadata metadata = new PhoneMetadata.Builder()
        //        .SetNationalPrefixForParsing("34")
        //        .SetGeneralDesc(new PhoneNumberDesc.Builder().SetNationalNumberPattern("\\d{4,8}").Build())
        //        .BuildPartial();
        //    StringBuilder numberToStrip = new StringBuilder("34356778");
        //    String strippedNumber = "356778";
        //    Assert.True(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(numberToStrip, metadata, null));
        //    Assert.Equal(strippedNumber, numberToStrip.ToString(),
        //        "Should have had national prefix stripped.");
        //    // Retry stripping - now the number should not start with the national prefix, so no more
        //    // stripping should occur.
        //    Assert.False(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(numberToStrip, metadata, null));
        //    Assert.Equal(strippedNumber, numberToStrip.ToString(),
        //        "Should have had no change - no national prefix present.");
        //    // Some countries have no national prefix. Repeat Test with none specified.
        //    metadata = Update(metadata).SetNationalPrefixForParsing("").BuildPartial();
        //    Assert.False(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(numberToStrip, metadata, null));
        //    Assert.Equal(strippedNumber, numberToStrip.ToString(),
        //        "Should not strip anything with empty national prefix.");
        //    // If the resultant number doesn't match the national rule, it shouldn't be stripped.
        //    metadata = Update(metadata).SetNationalPrefixForParsing("3").BuildPartial();
        //    numberToStrip = new StringBuilder("3123");
        //    strippedNumber = "3123";
        //    Assert.False(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(numberToStrip, metadata, null));
        //    Assert.Equal(strippedNumber, numberToStrip.ToString(),
        //        "Should have had no change - after stripping, it wouldn't have matched " +
        //        "the national rule.");
        //    // Test extracting carrier selection code.
        //    metadata = Update(metadata).SetNationalPrefixForParsing("0(81)?").BuildPartial();
        //    numberToStrip = new StringBuilder("08122123456");
        //    strippedNumber = "22123456";
        //    StringBuilder carrierCode = new StringBuilder();
        //    Assert.True(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(
        //        numberToStrip, metadata, carrierCode));
        //    Assert.Equal("81", carrierCode.ToString());
        //    Assert.Equal(strippedNumber, numberToStrip.ToString(),
        //        "Should have had national prefix and carrier code stripped.");
        //    // If there was a transform rule, check it was applied.
        //    // Note True a capturing group is present here.
        //    metadata = Update(metadata).SetNationalPrefixTransformRule("5${1}5")
        //        .SetNationalPrefixForParsing("0(\\d{2})").BuildPartial();
        //    numberToStrip = new StringBuilder("031123");
        //    String transformedNumber = "5315123";
        //    Assert.True(phoneUtil.MaybeStripNationalPrefixAndCarrierCode(numberToStrip, metadata, null));
        //    Assert.Equal(transformedNumber, numberToStrip.ToString(),
        //        "Should transform the 031 to a 5315.");
        //}

        //[Fact]
        //public void TestMaybeStripInternationalPrefix()
        //{
        //    String internationalPrefix = "00[39]";
        //    StringBuilder numberToStrip = new StringBuilder("0034567700-3898003");
        //    // Note the dash is removed as part of the normalization.
        //    StringBuilder strippedNumber = new StringBuilder("45677003898003");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_IDD,
        //        phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //            internationalPrefix));
        //    Assert.Equal(strippedNumber.ToString(), numberToStrip.ToString(),
        //        "The number supplied was not stripped of its international prefix.");
        //    // Now the number no longer starts with an IDD prefix, so it should now report
        //    // FROM_DEFAULT_COUNTRY.
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY,
        //        phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //            internationalPrefix));

        //    numberToStrip = new StringBuilder("00945677003898003");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_IDD,
        //        phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //            internationalPrefix));
        //    Assert.Equal(strippedNumber.ToString(), numberToStrip.ToString(),
        //        "The number supplied was not stripped of its international prefix.");
        //    // Test it works when the international prefix is broken up by spaces.
        //    numberToStrip = new StringBuilder("00 9 45677003898003");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_IDD,
        //        phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //            internationalPrefix));
        //    Assert.Equal(strippedNumber.ToString(), numberToStrip.ToString(),
        //        "The number supplied was not stripped of its international prefix.");
        //    // Now the number no longer starts with an IDD prefix, so it should now report
        //    // FROM_DEFAULT_COUNTRY.
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY,
        //        phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //            internationalPrefix));

        //    // Test the + symbol is also recognised and stripped.
        //    numberToStrip = new StringBuilder("+45677003898003");
        //    strippedNumber = new StringBuilder("45677003898003");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN,
        //    phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //                                                 internationalPrefix));
        //    Assert.Equal(strippedNumber.ToString(), numberToStrip.ToString(),
        //        "The number supplied was not stripped of the plus symbol.");

        //    // If the number afterwards is a zero, we should not strip this - no country calling code begins
        //    // with 0.
        //    numberToStrip = new StringBuilder("0090112-3123");
        //    strippedNumber = new StringBuilder("00901123123");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY,
        //    phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //                                                 internationalPrefix));
        //    Assert.Equal(strippedNumber.ToString(), numberToStrip.ToString(),
        //        "The number supplied had a 0 after the match so shouldn't be stripped.");
        //    // Here the 0 is separated by a space from the IDD.
        //    numberToStrip = new StringBuilder("009 0-112-3123");
        //    Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY,
        //    phoneUtil.MaybeStripInternationalPrefixAndNormalize(numberToStrip,
        //                                                 internationalPrefix));
        //}

        //[Fact]
        //public void TestMaybeExtractCountryCode()
        //{
        //    var number = new PhoneNumber.Builder();
        //    PhoneMetadata metadata = phoneUtil.GetMetadataForRegion(RegionCode.US);
        //    // Note True for the US, the IDD is 011.
        //    try
        //    {
        //        String phoneNumber = "011112-3456789";
        //        String strippedNumber = "123456789";
        //        int countryCallingCode = 1;
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(countryCallingCode,
        //            phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true, number),
        //            "Did not extract country calling code " + countryCallingCode + " correctly.");
        //        Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_IDD, number.CountryCodeSource,
        //            "Did not figure out CountryCodeSource correctly");
        //        // Should strip and normalize national significant number.
        //        Assert.Equal(strippedNumber,
        //            numberToFill.ToString(),
        //            "Did not strip off the country calling code correctly.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "+6423456789";
        //        int countryCallingCode = 64;
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(countryCallingCode,
        //            phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true, number),
        //            "Did not extract country calling code " + countryCallingCode + " correctly.");
        //        Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN, number.CountryCodeSource,
        //            "Did not figure out CountryCodeSource correctly");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "+80012345678";
        //        int countryCallingCode = 800;
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(countryCallingCode,
        //           phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true, number),
        //           "Did not extract country calling code " + countryCallingCode + " correctly.");
        //        Assert.Equal(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN, number.CountryCodeSource,
        //            "Did not figure out CountryCodeSource correctly");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "2345-6789";
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(
        //        0,
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true, number),
        //        "Should not have extracted a country calling code - no international prefix present.");
        //        Assert.Equal(
        //        PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY, number.CountryCodeSource,
        //        "Did not figure out CountryCodeSource correctly");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "0119991123456789";
        //        StringBuilder numberToFill = new StringBuilder();
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true, number);
        //        Assert.Fail("Should have thrown an exception, no valid country calling code present.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected.
        //        Assert.Equal(
        //        ErrorType.INVALID_COUNTRY_CODE,
        //        e.ErrorType,
        //        "Wrong error type stored in exception.");
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "(1 610) 619 4466";
        //        int countryCallingCode = 1;
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(
        //        countryCallingCode,
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true,
        //        number),
        //        "Should have extracted the country calling code of the region passed in");
        //        Assert.Equal(
        //        PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITHOUT_PLUS_SIGN,
        //        number.CountryCodeSource,
        //        "Did not figure out CountryCodeSource correctly");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "(1 610) 619 4466";
        //        int countryCallingCode = 1;
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(
        //        countryCallingCode,
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, false,
        //        number),
        //        "Should have extracted the country calling code of the region passed in");
        //        Assert.False(number.HasCountryCodeSource, "Should not contain CountryCodeSource.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "(1 610) 619 446";
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(
        //        0,
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, false,
        //        number),
        //        "Should not have extracted a country calling code - invalid number after " +
        //        "extraction of uncertain country calling code.");
        //        Assert.False(number.HasCountryCodeSource, "Should not contain CountryCodeSource.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //    number = new PhoneNumber.Builder();
        //    try
        //    {
        //        String phoneNumber = "(1 610) 619";
        //        StringBuilder numberToFill = new StringBuilder();
        //        Assert.Equal(
        //        0,
        //        phoneUtil.MaybeExtractCountryCode(phoneNumber, metadata, numberToFill, true,
        //        number),
        //        "Should not have extracted a country calling code - too short number both " +
        //        "before and after extraction of uncertain country calling code.");
        //        Assert.Equal(
        //        PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY, number.CountryCodeSource,
        //        "Did not figure out CountryCodeSource correctly");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        Assert.Fail("Should not have thrown an exception: " + e.ToString());
        //    }
        //}

        [Fact]
        public void TestParseNationalNumber()
        {
            // National prefix attached.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("033316005", RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("33316005", RegionCode.NZ));
            // National prefix attached and some formatting present.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("03-331 6005", RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("03 331 6005", RegionCode.NZ));
            // Test parsing RFC3966 format with a phone context.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:03-331-6005;phone-context=+64", RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:331-6005;phone-context=+64-3", RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:331-6005;phone-context=+64-3", RegionCode.US));

            // Test parsing RFC3966 format with optional user-defined parameters. The parameters will appear
            // after the context if present.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:03-331-6005;phone-context=+64;a=%A1",
                RegionCode.NZ));
            // Test parsing RFC3966 with an ISDN subaddress.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:03-331-6005;isub=12345;phone-context=+64",
                RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:+64-3-331-6005;isub=12345", RegionCode.NZ));

            // Testing international prefixes.
            // Should strip country calling code.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("0064 3 331 6005", RegionCode.NZ));
            // Try again, but this time we have an international number with Region Code US. It should
            // recognise the country calling code and parse accordingly.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("01164 3 331 6005", RegionCode.US));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+64 3 331 6005", RegionCode.US));

            // We should ignore the leading plus here, since it is not followed by a valid country code but
            // instead is followed by the IDD for the US.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+01164 3 331 6005", RegionCode.US));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+0064 3 331 6005", RegionCode.NZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+ 00 64 3 331 6005", RegionCode.NZ));

            Assert.Equal(US_LOCAL_NUMBER,
                phoneUtil.Parse("tel:253-0000;phone-context=www.google.com", RegionCode.US));
            Assert.Equal(US_LOCAL_NUMBER,
                phoneUtil.Parse("tel:253-0000;isub=12345;phone-context=www.google.com", RegionCode.US));
            // This is invalid because no "+" sign is present as part of phone-context. The phone context
            // is simply ignored in this case just as if it contains a domain.
            Assert.Equal(US_LOCAL_NUMBER,
                phoneUtil.Parse("tel:2530000;isub=12345;phone-context=1-650", RegionCode.US));
            Assert.Equal(US_LOCAL_NUMBER,
                phoneUtil.Parse("tel:2530000;isub=12345;phone-context=1234.com", RegionCode.US));

            PhoneNumber nzNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(64123456L).Build();
            Assert.Equal(nzNumber, phoneUtil.Parse("64(0)64123456", RegionCode.NZ));
            // Check True using a "/" is fine in a phone number.
            Assert.Equal(DE_NUMBER, phoneUtil.Parse("301/23456", RegionCode.DE));

            // Check it doesn't use the '1' as a country calling code when parsing if the phone number was
            // already possible.
            PhoneNumber usNumber = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(1234567890L).Build();
            Assert.Equal(usNumber, phoneUtil.Parse("123-456-7890", RegionCode.US));

            // Test star numbers. Although this is not strictly valid, we would like to make sure we can
            // parse the output we produce when formatting the number.
            Assert.Equal(JP_STAR_NUMBER, phoneUtil.Parse("+81 *2345", RegionCode.JP));

            PhoneNumber shortNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(12L).Build();
            Assert.Equal(shortNumber, phoneUtil.Parse("12", RegionCode.NZ));
        }

        [Fact]
        public void TestParseNumberWithAlphaCharacters()
        {
            // Test case with alpha characters.
            PhoneNumber tollfreeNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(800332005L).Build();
            Assert.Equal(tollfreeNumber, phoneUtil.Parse("0800 DDA 005", RegionCode.NZ));
            PhoneNumber premiumNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(9003326005L).Build();
            Assert.Equal(premiumNumber, phoneUtil.Parse("0900 DDA 6005", RegionCode.NZ));
            // Not enough alpha characters for them to be considered intentional, so they are stripped.
            Assert.Equal(premiumNumber, phoneUtil.Parse("0900 332 6005a", RegionCode.NZ));
            Assert.Equal(premiumNumber, phoneUtil.Parse("0900 332 600a5", RegionCode.NZ));
            Assert.Equal(premiumNumber, phoneUtil.Parse("0900 332 600A5", RegionCode.NZ));
            Assert.Equal(premiumNumber, phoneUtil.Parse("0900 a332 600A5", RegionCode.NZ));
        }

        [Fact]
        public void TestParseMaliciousInput()
        {
            // Lots of leading + signs before the possible number.
            StringBuilder maliciousNumber = new StringBuilder(6000);
            for (int i = 0; i < 6000; i++)
                maliciousNumber.Append('+');
            maliciousNumber.Append("12222-33-244 extensioB 343+");

            var ex = Assert.ThrowsAny<NumberParseException>(() =>
            {
                phoneUtil.Parse(maliciousNumber.ToString(), RegionCode.US);
            });

            // Expected this exception.
            Assert.Equal(ErrorType.TOO_LONG, ex.ErrorType);

            StringBuilder maliciousNumberWithAlmostExt = new StringBuilder(6000);
            for (int i = 0; i < 350; i++)
                maliciousNumberWithAlmostExt.Append("200");
            maliciousNumberWithAlmostExt.Append(" extensiOB 345");

            ex = Assert.ThrowsAny<NumberParseException>(() =>
            {
                phoneUtil.Parse(maliciousNumberWithAlmostExt.ToString(), RegionCode.US);
            });
            
            // Expected this exception.
            Assert.Equal(ErrorType.TOO_LONG, ex.ErrorType);
        }

        [Fact]
        public void TestParseWithInternationalPrefixes()
        {
            Assert.Equal(US_NUMBER, phoneUtil.Parse("+1 (650) 253-0000", RegionCode.NZ));
            Assert.Equal(INTERNATIONAL_TOLL_FREE, phoneUtil.Parse("011 800 1234 5678", RegionCode.US));
            Assert.Equal(US_NUMBER, phoneUtil.Parse("1-650-253-0000", RegionCode.US));
            // Calling the US number from Singapore by using different service providers
            // 1st Test: calling using SingTel IDD service (IDD is 001)
            Assert.Equal(US_NUMBER, phoneUtil.Parse("0011-650-253-0000", RegionCode.SG));
            // 2nd Test: calling using StarHub IDD service (IDD is 008)
            Assert.Equal(US_NUMBER, phoneUtil.Parse("0081-650-253-0000", RegionCode.SG));
            // 3rd Test: calling using SingTel V019 service (IDD is 019)
            Assert.Equal(US_NUMBER, phoneUtil.Parse("0191-650-253-0000", RegionCode.SG));
            // Calling the US number from Poland
            Assert.Equal(US_NUMBER, phoneUtil.Parse("0~01-650-253-0000", RegionCode.PL));
            // Using "++" at the start.
            Assert.Equal(US_NUMBER, phoneUtil.Parse("++1 (650) 253-0000", RegionCode.PL));
            // Using a very strange decimal digit range (Mongolian digits).
            Assert.Equal(US_NUMBER, phoneUtil.Parse("\u1811 \u1816\u1815\u1810 " +
                "\u1812\u1815\u1813 \u1810\u1810\u1810\u1810",
                RegionCode.US));
        }

        [Fact]
        public void TestParseNonAscii()
        {
            // Using a full-width plus sign.
            Assert.Equal(US_NUMBER, phoneUtil.Parse("\uFF0B1 (650) 253-0000", RegionCode.SG));
            // Using a soft hyphen U+00AD.
            Assert.Equal(US_NUMBER, phoneUtil.Parse("1 (650) 253\u00AD-0000", RegionCode.US));
            // The whole number, including punctuation, is here represented in full-width form.
            Assert.Equal(US_NUMBER, phoneUtil.Parse("\uFF0B\uFF11\u3000\uFF08\uFF16\uFF15\uFF10\uFF09" +
                                                "\u3000\uFF12\uFF15\uFF13\uFF0D\uFF10\uFF10\uFF10" +
                                                "\uFF10",
                                                RegionCode.SG));
            // Using U+30FC dash instead.
            Assert.Equal(US_NUMBER, phoneUtil.Parse("\uFF0B\uFF11\u3000\uFF08\uFF16\uFF15\uFF10\uFF09" +
                                                "\u3000\uFF12\uFF15\uFF13\u30FC\uFF10\uFF10\uFF10" +
                                                "\uFF10",
                                                RegionCode.SG));
        }

        [Fact]
        public void TestParseWithLeadingZero()
        {
            Assert.Equal(IT_NUMBER, phoneUtil.Parse("+39 02-36618 300", RegionCode.NZ));
            Assert.Equal(IT_NUMBER, phoneUtil.Parse("02-36618 300", RegionCode.IT));

            Assert.Equal(IT_MOBILE, phoneUtil.Parse("345 678 901", RegionCode.IT));
        }

        [Fact]
        public void TestParseNationalNumberArgentina()
        {
            // Test parsing mobile numbers of Argentina.
            var arNumber = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(93435551212L).Build();
            Assert.Equal(arNumber, phoneUtil.Parse("+54 9 343 555 1212", RegionCode.AR));
            Assert.Equal(arNumber, phoneUtil.Parse("0343 15 555 1212", RegionCode.AR));

            arNumber = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(93715654320L).Build();
            Assert.Equal(arNumber, phoneUtil.Parse("+54 9 3715 65 4320", RegionCode.AR));
            Assert.Equal(arNumber, phoneUtil.Parse("03715 15 65 4320", RegionCode.AR));
            Assert.Equal(AR_MOBILE, phoneUtil.Parse("911 876 54321", RegionCode.AR));

            // Test parsing fixed-line numbers of Argentina.
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("+54 11 8765 4321", RegionCode.AR));
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("011 8765 4321", RegionCode.AR));

            arNumber = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(3715654321L).Build();
            Assert.Equal(arNumber, phoneUtil.Parse("+54 3715 65 4321", RegionCode.AR));
            Assert.Equal(arNumber, phoneUtil.Parse("03715 65 4321", RegionCode.AR));

            arNumber = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(2312340000L).Build();
            Assert.Equal(arNumber, phoneUtil.Parse("+54 23 1234 0000", RegionCode.AR));
            Assert.Equal(arNumber, phoneUtil.Parse("023 1234 0000", RegionCode.AR));
        }

        [Fact]
        public void TestParseWithXInNumber()
        {
            // Test True having an 'x' in the phone number at the start is ok and True it just gets removed.
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("01187654321", RegionCode.AR));
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("(0) 1187654321", RegionCode.AR));
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("0 1187654321", RegionCode.AR));
            Assert.Equal(AR_NUMBER, phoneUtil.Parse("(0xx) 1187654321", RegionCode.AR));
            var arFromUs = new PhoneNumber.Builder()
                .SetCountryCode(54).SetNationalNumber(81429712L).Build();
            // This Test is intentionally constructed such True the number of digit after xx is larger than
            // 7, so True the number won't be mistakenly treated as an extension, as we allow extensions up
            // to 7 digits. This assumption is okay for now as all the countries where a carrier selection
            // code is written in the form of xx have a national significant number of length larger than 7.
            Assert.Equal(arFromUs, phoneUtil.Parse("011xx5481429712", RegionCode.US));
        }

        [Fact]
        public void TestParseNumbersMexico()
        {
            // Test parsing fixed-line numbers of Mexico.
            PhoneNumber mxNumber = new PhoneNumber.Builder()
                .SetCountryCode(52).SetNationalNumber(4499780001L).Build();
            Assert.Equal(mxNumber, phoneUtil.Parse("+52 (449)978-0001", RegionCode.MX));
            Assert.Equal(mxNumber, phoneUtil.Parse("01 (449)978-0001", RegionCode.MX));
            Assert.Equal(mxNumber, phoneUtil.Parse("(449)978-0001", RegionCode.MX));

            // Test parsing mobile numbers of Mexico.
            mxNumber = new PhoneNumber.Builder()
                .SetCountryCode(52).SetNationalNumber(13312345678L).Build();
            Assert.Equal(mxNumber, phoneUtil.Parse("+52 1 33 1234-5678", RegionCode.MX));
            Assert.Equal(mxNumber, phoneUtil.Parse("044 (33) 1234-5678", RegionCode.MX));
            Assert.Equal(mxNumber, phoneUtil.Parse("045 33 1234-5678", RegionCode.MX));
        }

        //[Fact]
        //public void TestFailedParseOnInvalidNumbers()
        //{
        //    try
        //    {
        //        String sentencePhoneNumber = "This is not a phone number";
        //        phoneUtil.Parse(sentencePhoneNumber, RegionCode.NZ);
        //        Assert.Fail("This should not parse without throwing an exception " + sentencePhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.NOT_A_NUMBER,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String sentencePhoneNumber = "1 Still not a number";
        //        phoneUtil.Parse(sentencePhoneNumber, RegionCode.NZ);
        //        Assert.Fail("This should not parse without throwing an exception " + sentencePhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.NOT_A_NUMBER,
        //               e.ErrorType,
        //               "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String sentencePhoneNumber = "1 MICROSOFT";
        //        phoneUtil.Parse(sentencePhoneNumber, RegionCode.NZ);
        //        Assert.Fail("This should not parse without throwing an exception " + sentencePhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.NOT_A_NUMBER,
        //               e.ErrorType,
        //               "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String sentencePhoneNumber = "12 MICROSOFT";
        //        phoneUtil.Parse(sentencePhoneNumber, RegionCode.NZ);
        //        Assert.Fail("This should not parse without throwing an exception " + sentencePhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.NOT_A_NUMBER,
        //               e.ErrorType,
        //               "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String tooLongPhoneNumber = "01495 72553301873 810104";
        //        phoneUtil.Parse(tooLongPhoneNumber, RegionCode.GB);
        //        Assert.Fail("This should not parse without throwing an exception " + tooLongPhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_LONG,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String plusMinusPhoneNumber = "+---";
        //        phoneUtil.Parse(plusMinusPhoneNumber, RegionCode.DE);
        //        Assert.Fail("This should not parse without throwing an exception " + plusMinusPhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.NOT_A_NUMBER,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String plusStar = "+***";
        //        phoneUtil.Parse(plusStar, RegionCode.DE);
        //        Assert.Fail("This should not parse without throwing an exception " + plusStar);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //           ErrorType.NOT_A_NUMBER,
        //           e.ErrorType,
        //           "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String plusStarPhoneNumber = "+*******91";
        //        phoneUtil.Parse(plusStarPhoneNumber, RegionCode.DE);
        //        Assert.Fail("This should not parse without throwing an exception " + plusStarPhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //           ErrorType.NOT_A_NUMBER,
        //           e.ErrorType,
        //           "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String tooShortPhoneNumber = "+49 0";
        //        phoneUtil.Parse(tooShortPhoneNumber, RegionCode.DE);
        //        Assert.Fail("This should not parse without throwing an exception " + tooShortPhoneNumber);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_SHORT_NSN,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String invalidCountryCode = "+210 3456 56789";
        //        phoneUtil.Parse(invalidCountryCode, RegionCode.NZ);
        //        Assert.Fail("This is not a recognised region code: should fail: " + invalidCountryCode);
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //            ErrorType.INVALID_COUNTRY_CODE,
        //           e.ErrorType,
        //           "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String plusAndIddAndInvalidCountryCode = "+ 00 210 3 331 6005";
        //        phoneUtil.Parse(plusAndIddAndInvalidCountryCode, RegionCode.NZ);
        //        Assert.Fail("This should not parse without throwing an exception.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception. 00 is a correct IDD, but 210 is not a valid country code.
        //        Assert.Equal(
        //                     ErrorType.INVALID_COUNTRY_CODE,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "123 456 7890";
        //        phoneUtil.Parse(someNumber, RegionCode.ZZ);
        //        Assert.Fail("'Unknown' region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.INVALID_COUNTRY_CODE,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "123 456 7890";
        //        phoneUtil.Parse(someNumber, RegionCode.CS);
        //        Assert.Fail("Deprecated region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.INVALID_COUNTRY_CODE,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "123 456 7890";
        //        phoneUtil.Parse(someNumber, null);
        //        Assert.Fail("Null region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.INVALID_COUNTRY_CODE,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "0044------";
        //        phoneUtil.Parse(someNumber, RegionCode.GB);
        //        Assert.Fail("No number provided, only region code: should fail");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_SHORT_AFTER_IDD,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "0044";
        //        phoneUtil.Parse(someNumber, RegionCode.GB);
        //        Assert.Fail("No number provided, only region code: should fail");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_SHORT_AFTER_IDD,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "011";
        //        phoneUtil.Parse(someNumber, RegionCode.US);
        //        Assert.Fail("Only IDD provided - should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_SHORT_AFTER_IDD,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String someNumber = "0119";
        //        phoneUtil.Parse(someNumber, RegionCode.US);
        //        Assert.Fail("Only IDD provided and then 9 - should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.TOO_SHORT_AFTER_IDD,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String emptyNumber = "";
        //        // Invalid region.
        //        phoneUtil.Parse(emptyNumber, RegionCode.ZZ);
        //        Assert.Fail("Empty string - should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.NOT_A_NUMBER,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    try
        //    {
        //        String nullNumber = null;
        //        // Invalid region.
        //        phoneUtil.Parse(nullNumber, RegionCode.ZZ);
        //        Assert.Fail("Null string - should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.NOT_A_NUMBER,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        Assert.Fail("Null string - but should not throw a null pointer exception.");
        //    }
        //    try
        //    {
        //        String nullNumber = null;
        //        phoneUtil.Parse(nullNumber, RegionCode.US);
        //        Assert.Fail("Null string - should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //                     ErrorType.NOT_A_NUMBER,
        //                     e.ErrorType,
        //                     "Wrong error type stored in exception.");
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        Assert.Fail("Null string - but should not throw a null pointer exception.");
        //    }
        //    try
        //    {
        //        String domainRfcPhoneContext = "tel:555-1234;phone-context=www.google.com";
        //        phoneUtil.Parse(domainRfcPhoneContext, RegionCode.ZZ);
        //        Assert.Fail("'Unknown' region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.INVALID_COUNTRY_CODE,
        //           e.ErrorType,
        //           "Wrong error type stored in exception.");
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        Assert.Fail("Domain provided for phone context - but should not throw a null pointer exception.");
        //    }
        //    try
        //    {
        //        // This is invalid because no "+" sign is present as part of phone-context. This should not
        //        // succeed in being parsed.
        //        String invalidRfcPhoneContext = "tel:555-1234;phone-context=1-331";
        //        phoneUtil.Parse(invalidRfcPhoneContext, RegionCode.ZZ);
        //        Assert.Fail("'Unknown' region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(ErrorType.INVALID_COUNTRY_CODE,
        //           e.ErrorType,
        //           "Wrong error type stored in exception.");
        //    }
        //}

        [Fact]
        public void TestParseNumbersWithPlusWithNoRegion()
        {
            // RegionCode.ZZ is allowed only if the number starts with a '+' - then the country calling code
            // can be calculated.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+64 3 331 6005", RegionCode.ZZ));
            // Test with full-width plus.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("\uFF0B64 3 331 6005", RegionCode.ZZ));
            // Test with normal plus but leading characters True need to be stripped.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("Tel: +64 3 331 6005", RegionCode.ZZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("+64 3 331 6005", null));
            Assert.Equal(INTERNATIONAL_TOLL_FREE, phoneUtil.Parse("+800 1234 5678", null));
            Assert.Equal(UNIVERSAL_PREMIUM_RATE, phoneUtil.Parse("+979 123 456 789", null));

            // Test parsing RFC3966 format with a phone context.
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:03-331-6005;phone-context=+64", RegionCode.ZZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("  tel:03-331-6005;phone-context=+64", RegionCode.ZZ));
            Assert.Equal(NZ_NUMBER, phoneUtil.Parse("tel:03-331-6005;isub=12345;phone-context=+64",
                RegionCode.ZZ));

            // It is important True we set the carrier code to an empty string, since we used
            // ParseAndKeepRawInput and no carrier code was found.
            PhoneNumber nzNumberWithRawInput = new PhoneNumber.Builder().MergeFrom(NZ_NUMBER)
                .SetRawInput("+64 3 331 6005")
                .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN)
                .SetPreferredDomesticCarrierCode("").Build();
            Assert.Equal(nzNumberWithRawInput, phoneUtil.ParseAndKeepRawInput("+64 3 331 6005",
                                                                      RegionCode.ZZ));
            // Null is also allowed for the region code in these cases.
            Assert.Equal(nzNumberWithRawInput, phoneUtil.ParseAndKeepRawInput("+64 3 331 6005", null));
        }

        [Fact]
        public void TestParseExtensions()
        {
            PhoneNumber nzNumber = new PhoneNumber.Builder()
                .SetCountryCode(64).SetNationalNumber(33316005L).SetExtension("3456").Build();
            Assert.Equal(nzNumber, phoneUtil.Parse("03 331 6005 ext 3456", RegionCode.NZ));
            Assert.Equal(nzNumber, phoneUtil.Parse("03-3316005x3456", RegionCode.NZ));
            Assert.Equal(nzNumber, phoneUtil.Parse("03-3316005 int.3456", RegionCode.NZ));
            Assert.Equal(nzNumber, phoneUtil.Parse("03 3316005 #3456", RegionCode.NZ));
            // Test the following do not extract extensions:
            Assert.Equal(ALPHA_NUMERIC_NUMBER, phoneUtil.Parse("1800 six-flags", RegionCode.US));
            Assert.Equal(ALPHA_NUMERIC_NUMBER, phoneUtil.Parse("1800 SIX FLAGS", RegionCode.US));
            Assert.Equal(ALPHA_NUMERIC_NUMBER, phoneUtil.Parse("0~0 1800 7493 5247", RegionCode.PL));
            Assert.Equal(ALPHA_NUMERIC_NUMBER, phoneUtil.Parse("(1800) 7493.5247", RegionCode.US));
            // Check True the last instance of an extension token is matched.
            PhoneNumber extnNumber = new PhoneNumber.Builder().MergeFrom(ALPHA_NUMERIC_NUMBER).SetExtension("1234").Build();
            Assert.Equal(extnNumber, phoneUtil.Parse("0~0 1800 7493 5247 ~1234", RegionCode.PL));
            // Verifying bug-fix where the last digit of a number was previously omitted if it was a 0 when
            // extracting the extension. Also verifying a few different cases of extensions.
            PhoneNumber ukNumber = new PhoneNumber.Builder()
                .SetCountryCode(44).SetNationalNumber(2034567890L).SetExtension("456").Build();
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890x456", RegionCode.NZ));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890x456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890 x456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890 X456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890 X 456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890 X  456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890 x 456  ", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44 2034567890  X 456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("+44-2034567890;ext=456", RegionCode.GB));
            Assert.Equal(ukNumber, phoneUtil.Parse("tel:2034567890;ext=456;phone-context=+44",
                                           RegionCode.ZZ));
            // Full-width extension, "extn" only.
            Assert.Equal(ukNumber, phoneUtil.Parse("+442034567890\uFF45\uFF58\uFF54\uFF4E456",
                RegionCode.GB));
            // "xtn" only.
            Assert.Equal(ukNumber, phoneUtil.Parse("+442034567890\uFF58\uFF54\uFF4E456",
                RegionCode.GB));
            // "xt" only.
            Assert.Equal(ukNumber, phoneUtil.Parse("+442034567890\uFF58\uFF54456",
                RegionCode.GB));

            PhoneNumber usWithExtension = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(8009013355L).SetExtension("7246433").Build();
            Assert.Equal(usWithExtension, phoneUtil.Parse("(800) 901-3355 x 7246433", RegionCode.US));
            Assert.Equal(usWithExtension, phoneUtil.Parse("(800) 901-3355 , ext 7246433", RegionCode.US));
            Assert.Equal(usWithExtension,
                     phoneUtil.Parse("(800) 901-3355 ,extension 7246433", RegionCode.US));
            Assert.Equal(usWithExtension,
                     phoneUtil.Parse("(800) 901-3355 ,extensi\u00F3n 7246433", RegionCode.US));
            // Repeat with the small letter o with acute accent created by combining characters.
            Assert.Equal(usWithExtension,
                     phoneUtil.Parse("(800) 901-3355 ,extensio\u0301n 7246433", RegionCode.US));
            Assert.Equal(usWithExtension, phoneUtil.Parse("(800) 901-3355 , 7246433", RegionCode.US));
            Assert.Equal(usWithExtension, phoneUtil.Parse("(800) 901-3355 ext: 7246433", RegionCode.US));

            // Test True if a number has two extensions specified, we ignore the second.
            PhoneNumber usWithTwoExtensionsNumber = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(2121231234L).SetExtension("508").Build();
            Assert.Equal(usWithTwoExtensionsNumber, phoneUtil.Parse("(212)123-1234 x508/x1234",
                                                                RegionCode.US));
            Assert.Equal(usWithTwoExtensionsNumber, phoneUtil.Parse("(212)123-1234 x508/ x1234",
                                                                RegionCode.US));
            Assert.Equal(usWithTwoExtensionsNumber, phoneUtil.Parse("(212)123-1234 x508\\x1234",
                                                                RegionCode.US));

            // Test parsing numbers in the form (645) 123-1234-910# works, where the last 3 digits before
            // the # are an extension.
            usWithExtension = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6451231234L).SetExtension("910").Build();
            Assert.Equal(usWithExtension, phoneUtil.Parse("+1 (645) 123 1234-910#", RegionCode.US));
            // Retry with the same number in a slightly different format.
            Assert.Equal(usWithExtension, phoneUtil.Parse("+1 (645) 123 1234 ext. 910#", RegionCode.US));
        }

        //[Fact]
        //public void TestParseAndKeepRaw()
        //{
        //    PhoneNumber alphaNumericNumber = new PhoneNumber.Builder().MergeFrom(ALPHA_NUMERIC_NUMBER)
        //        .SetRawInput("800 six-flags")
        //        .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY)
        //        .SetPreferredDomesticCarrierCode("").Build();
        //    Assert.Equal(alphaNumericNumber,
        //        phoneUtil.ParseAndKeepRawInput("800 six-flags", RegionCode.US));

        //    PhoneNumber shorterAlphaNumber = new PhoneNumber.Builder()
        //        .SetCountryCode(1)
        //        .SetNationalNumber(8007493524L)
        //        .SetRawInput("1800 six-flag")
        //        .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITHOUT_PLUS_SIGN)
        //        .SetPreferredDomesticCarrierCode("").Build();
        //    Assert.Equal(shorterAlphaNumber,
        //        phoneUtil.ParseAndKeepRawInput("1800 six-flag", RegionCode.US));

        //    shorterAlphaNumber = Update(shorterAlphaNumber).SetRawInput("+1800 six-flag").
        //        SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN).Build();
        //    Assert.Equal(shorterAlphaNumber,
        //        phoneUtil.ParseAndKeepRawInput("+1800 six-flag", RegionCode.NZ));

        //    shorterAlphaNumber = Update(shorterAlphaNumber).SetRawInput("001800 six-flag").
        //        SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_IDD).Build();
        //    Assert.Equal(shorterAlphaNumber,
        //        phoneUtil.ParseAndKeepRawInput("001800 six-flag", RegionCode.NZ));

        //    // Invalid region code supplied.
        //    try
        //    {
        //        phoneUtil.ParseAndKeepRawInput("123 456 7890", RegionCode.CS);
        //        Assert.Fail("Deprecated region code not allowed: should fail.");
        //    }
        //    catch (NumberParseException e)
        //    {
        //        // Expected this exception.
        //        Assert.Equal(
        //            ErrorType.INVALID_COUNTRY_CODE,
        //            e.ErrorType,
        //            "Wrong error type stored in exception.");
        //    }

        //    PhoneNumber koreanNumber = new PhoneNumber.Builder()
        //        .SetCountryCode(82).SetNationalNumber(22123456).SetRawInput("08122123456").
        //        SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY).
        //        SetPreferredDomesticCarrierCode("81").Build();
        //    Assert.Equal(koreanNumber, phoneUtil.ParseAndKeepRawInput("08122123456", RegionCode.KR));
        //}

        //[Fact]
        //public void TestCountryWithNoNumberDesc()
        //{
        //    // Andorra is a country where we don't have PhoneNumberDesc info in the metadata.
        //    PhoneNumber adNumber = new PhoneNumber.Builder()
        //        .SetCountryCode(376).SetNationalNumber(12345L).Build();
        //    Assert.Equal("+376 12345", phoneUtil.Format(adNumber, PhoneNumberFormat.INTERNATIONAL));
        //    Assert.Equal("+37612345", phoneUtil.Format(adNumber, PhoneNumberFormat.E164));
        //    Assert.Equal("12345", phoneUtil.Format(adNumber, PhoneNumberFormat.NATIONAL));
        //    Assert.Equal(PhoneNumberType.UNKNOWN, phoneUtil.GetNumberType(adNumber));
        //    Assert.True(phoneUtil.IsValidNumber(adNumber));

        //    // Test dialing a US number from within Andorra.
        //    Assert.Equal("00 1 650 253 0000",
        //    phoneUtil.FormatOutOfCountryCallingNumber(US_NUMBER, RegionCode.AD));
        //}

        //[Fact]
        //public void TestUnknownCountryCallingCodeForValidation()
        //{
        //    PhoneNumber invalidNumber = new PhoneNumber.Builder()
        //        .SetCountryCode(0).SetNationalNumber(1234L).Build();
        //    Assert.False(phoneUtil.IsValidNumber(invalidNumber));
        //}

        //[Fact]
        //public void TestIsNumberMatchMatches()
        //{
        //    // Test simple matches where formatting is different, or leading zeroes, or country calling code
        //    // has been specified.
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+64 3 331 6005", "+64 03 331 6005"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //         phoneUtil.IsNumberMatch("+800 1234 5678", "+80012345678"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+64 03 331-6005", "+64 03331 6005"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+643 331-6005", "+64033316005"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+643 331-6005", "+6433316005"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+64 3 331-6005", "+6433316005"));
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //         phoneUtil.IsNumberMatch("+64 3 331-6005", "tel:+64-3-331-6005;isub=123"));
        //    // Test alpha numbers.
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+1800 siX-Flags", "+1 800 7493 5247"));
        //    // Test numbers with extensions.
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch("+64 3 331-6005 extn 1234", "+6433316005#1234"));
        //    // Test proto buffers.
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch(NZ_NUMBER, "+6403 331 6005"));

        //    PhoneNumber nzNumber = new PhoneNumber.Builder().MergeFrom(NZ_NUMBER).SetExtension("3456").Build();
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch(nzNumber, "+643 331 6005 ext 3456"));
        //    // Check empty extensions are ignored.
        //    nzNumber = Update(nzNumber).SetExtension("").Build();
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch(nzNumber, "+6403 331 6005"));
        //    // Check variant with two proto buffers.
        //    Assert.Equal(
        //        PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch(nzNumber, NZ_NUMBER),
        //        "Number " + nzNumber.ToString() + " did not match " + NZ_NUMBER.ToString());

        //    // Check raw_input, country_code_source and preferred_domestic_carrier_code are ignored.
        //    PhoneNumber brNumberOne = new PhoneNumber.Builder()
        //        .SetCountryCode(55).SetNationalNumber(3121286979L)
        //        .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN)
        //        .SetPreferredDomesticCarrierCode("12").SetRawInput("012 3121286979").Build();
        //    PhoneNumber brNumberTwo = new PhoneNumber.Builder()
        //        .SetCountryCode(55).SetNationalNumber(3121286979L)
        //        .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY)
        //        .SetPreferredDomesticCarrierCode("14").SetRawInput("143121286979").Build();
        //    Assert.Equal(PhoneNumberUtil.MatchType.EXACT_MATCH,
        //        phoneUtil.IsNumberMatch(brNumberOne, brNumberTwo));
        //}

        [Fact]
        public void TestIsNumberMatchNonMatches()
        {
            // Non-matches.
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                phoneUtil.IsNumberMatch("03 331 6005", "03 331 6006"));
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                 phoneUtil.IsNumberMatch("+800 1234 5678", "+1 800 1234 5678"));
            // Different country calling code, partial number match.
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005", "+16433316005"));
            // Different country calling code, same number.
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005", "+6133316005"));
            // Extension different, all else the same.
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005 extn 1234", "0116433316005#1235"));
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                 phoneUtil.IsNumberMatch(
                     "+64 3 331-6005 extn 1234", "tel:+64-3-331-6005;ext=1235"));
            // NSN matches, but extension is different - not the same number.
            Assert.Equal(PhoneNumberUtil.MatchType.NO_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005 ext.1235", "3 331 6005#1234"));

            // Invalid numbers True can't be parsed.
            Assert.Equal(PhoneNumberUtil.MatchType.NOT_A_NUMBER,
                phoneUtil.IsNumberMatch("4", "3 331 6043"));
            Assert.Equal(PhoneNumberUtil.MatchType.NOT_A_NUMBER,
                phoneUtil.IsNumberMatch("+43", "+64 3 331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.NOT_A_NUMBER,
                phoneUtil.IsNumberMatch("+43", "64 3 331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.NOT_A_NUMBER,
                phoneUtil.IsNumberMatch("Dog", "64 3 331 6005"));
        }

        [Fact]
        public void TestIsNumberMatchNsnMatches()
        {
            // NSN matches.
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005", "03 331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                 phoneUtil.IsNumberMatch(
                     "+64 3 331-6005", "tel:03-331-6005;isub=1234;phone-context=abc.nz"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch(NZ_NUMBER, "03 331 6005"));
            // Here the second number possibly starts with the country calling code for New Zealand,
            // although we are unsure.
            PhoneNumber unchangedNzNumber = new PhoneNumber.Builder().MergeFrom(NZ_NUMBER).Build();
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch(unchangedNzNumber, "(64-3) 331 6005"));
            // Check the phone number proto was not edited during the method call.
            Assert.Equal(NZ_NUMBER, unchangedNzNumber);

            // Here, the 1 might be a national prefix, if we compare it to the US number, so the resultant
            // match is an NSN match.
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch(US_NUMBER, "1-650-253-0000"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch(US_NUMBER, "6502530000"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch("+1 650-253 0000", "1 650 253 0000"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch("1 650-253 0000", "1 650 253 0000"));
            Assert.Equal(PhoneNumberUtil.MatchType.NSN_MATCH,
                phoneUtil.IsNumberMatch("1 650-253 0000", "+1 650 253 0000"));
            // For this case, the match will be a short NSN match, because we cannot assume True the 1 might
            // be a national prefix, so don't remove it when parsing.
            PhoneNumber randomNumber = new PhoneNumber.Builder()
                .SetCountryCode(41).SetNationalNumber(6502530000L).Build();
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch(randomNumber, "1-650-253-0000"));
        }


        [Fact]
        public void TestIsNumberMatchShortNsnMatches()
        {
            // Short NSN matches with the country not specified for either one or both numbers.
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005", "331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                 phoneUtil.IsNumberMatch("+64 3 331-6005", "tel:331-6005;phone-context=abc.nz"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                 phoneUtil.IsNumberMatch("+64 3 331-6005",
                     "tel:331-6005;isub=1234;phone-context=abc.nz"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                 phoneUtil.IsNumberMatch("+64 3 331-6005",
                     "tel:331-6005;isub=1234;phone-context=abc.nz;a=%A1"));
            // We did not know True the "0" was a national prefix since neither number has a country code,
            // so this is considered a SHORT_NSN_MATCH.
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("3 331-6005", "03 331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("3 331-6005", "331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                 phoneUtil.IsNumberMatch("3 331-6005", "tel:331-6005;phone-context=abc.nz"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("3 331-6005", "+64 331 6005"));
            // Short NSN match with the country specified.
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("03 331-6005", "331 6005"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("1 234 345 6789", "345 6789"));
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("+1 (234) 345 6789", "345 6789"));
            // NSN matches, country calling code omitted for one number, extension missing for one.
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch("+64 3 331-6005", "3 331 6005#1234"));
            // One has Italian leading zero, one does not.
            PhoneNumber italianNumberOne = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(1234L).SetItalianLeadingZero(true).Build();
            PhoneNumber italianNumberTwo = new PhoneNumber.Builder()
                .SetCountryCode(39).SetNationalNumber(1234L).Build();
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch(italianNumberOne, italianNumberTwo));
            // One has an extension, the other has an extension of "".
            italianNumberOne = Update(italianNumberOne).SetExtension("1234").ClearItalianLeadingZero().Build();
            italianNumberTwo = Update(italianNumberTwo).SetExtension("").Build();
            Assert.Equal(PhoneNumberUtil.MatchType.SHORT_NSN_MATCH,
                phoneUtil.IsNumberMatch(italianNumberOne, italianNumberTwo));
        }

        [Fact]
        public void TestCanBeInternationallyDialled()
        {
            // We have no-international-dialling rules for the US in our Test metadata True say True
            // toll-free numbers cannot be dialled internationally.
            Assert.False(phoneUtil.CanBeInternationallyDialled(US_TOLLFREE));

            // Normal US numbers can be internationally dialled.
            Assert.True(phoneUtil.CanBeInternationallyDialled(US_NUMBER));

            // Invalid number.
            Assert.True(phoneUtil.CanBeInternationallyDialled(US_LOCAL_NUMBER));

            // We have no data for NZ - should return true.
            Assert.True(phoneUtil.CanBeInternationallyDialled(NZ_NUMBER));
            Assert.True(phoneUtil.CanBeInternationallyDialled(INTERNATIONAL_TOLL_FREE));
        }

        [Fact]
        public void TestIsAlphaNumber()
        {
            Assert.True(phoneUtil.IsAlphaNumber("1800 six-flags"));
            Assert.True(phoneUtil.IsAlphaNumber("1800 six-flags ext. 1234"));
            Assert.True(phoneUtil.IsAlphaNumber("+800 six-flags"));
            Assert.True(phoneUtil.IsAlphaNumber("180 six-flags"));
            Assert.False(phoneUtil.IsAlphaNumber("1800 123-1234"));
            Assert.False(phoneUtil.IsAlphaNumber("1 six-flags"));
            Assert.False(phoneUtil.IsAlphaNumber("18 six-flags"));
            Assert.False(phoneUtil.IsAlphaNumber("1800 123-1234 extension: 1234"));
            Assert.False(phoneUtil.IsAlphaNumber("+800 1234-1234"));
        }
    }
}
