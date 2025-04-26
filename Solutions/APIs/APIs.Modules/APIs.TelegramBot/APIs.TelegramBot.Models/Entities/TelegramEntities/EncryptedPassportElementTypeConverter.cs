using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class EncryptedPassportElementTypeConverter : JsonConverter<EncryptedPassportElementType>
    {
        public override void WriteJson(JsonWriter writer, EncryptedPassportElementType value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                EncryptedPassportElementType.PersonalDetails => "personal_details",
                EncryptedPassportElementType.Passport => "passport",
                EncryptedPassportElementType.DriverLicence => "driver_licence",
                EncryptedPassportElementType.IdentityCard => "identity_card",
                EncryptedPassportElementType.InternalPassport => "internal_passport",
                EncryptedPassportElementType.Address => "address",
                EncryptedPassportElementType.UtilityBill => "utility_bill",
                EncryptedPassportElementType.BankStatement => "bank_statement",
                EncryptedPassportElementType.RentalAgreement => "rental_agreement",
                EncryptedPassportElementType.PassportRegistration => "passport_registration",
                EncryptedPassportElementType.TemporaryRegistration => "temporary_registration",
                EncryptedPassportElementType.PhoneNumber => "phone_number",
                EncryptedPassportElementType.Email => "email",
                (EncryptedPassportElementType)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override EncryptedPassportElementType ReadJson(
            JsonReader reader,
            Type objectType,
        EncryptedPassportElementType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "personal_details" => EncryptedPassportElementType.PersonalDetails,
                "passport" => EncryptedPassportElementType.Passport,
                "driver_licence" => EncryptedPassportElementType.DriverLicence,
                "identity_card" => EncryptedPassportElementType.IdentityCard,
                "internal_passport" => EncryptedPassportElementType.InternalPassport,
                "address" => EncryptedPassportElementType.Address,
                "utility_bill" => EncryptedPassportElementType.UtilityBill,
                "bank_statement" => EncryptedPassportElementType.BankStatement,
                "rental_agreement" => EncryptedPassportElementType.RentalAgreement,
                "passport_registration" => EncryptedPassportElementType.PassportRegistration,
                "temporary_registration" => EncryptedPassportElementType.TemporaryRegistration,
                "phone_number" => EncryptedPassportElementType.PhoneNumber,
                "email" => EncryptedPassportElementType.Email,
                _ => 0,
            };
    }
}
