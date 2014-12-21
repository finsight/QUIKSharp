using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace QuikSharp.Tests {


    public class TypeWithNumberSerializedAsString {
        public int NormalInt { get; set; }
        [JsonConverter(typeof(ToStringConverter<int?>))]
        public int? AsString { get; set; }
    }

    public class TypeWithNumberDeSerializedAsString {
        public int NormalInt { get; set; }
        public string AsString { get; set; }
    }

    public class TypeWithDateTimeSerializedAsString {
        [JsonConverter(typeof(HHMMSSDateTimeConverter))]
        public DateTime? AsString { get; set; }
    }

    public class TypeWithDateTimeDeSerializedAsString {
        public string AsString { get; set; }
    }

    [TestFixture]
    public class TransactionSpecTest {
        
        /// <summary>
        /// Make sure that Buy or Sell is never set by default
        /// That would be a very stupid mistake, but such king of mistakes are the most
        /// dangerous because one never believes he will make them
        /// </summary>
        [Test]
        public void UndefinedEnumCannotBeSerialized() {
            var op = (TransactionOperation)10;
            var json = op.ToJson();
            Assert.AreEqual(json, "null");
            Console.WriteLine(json);
            op = (TransactionOperation)1;
            json = op.ToJson();
            Console.WriteLine(json);
            Assert.AreEqual(json, "B".ToJson());

            var act = (TransactionAction)0;
            json = act.ToJson();
            Console.WriteLine(json);
            Assert.AreEqual(json, "null");

            var yesNo = (YesOrNo)0;
            json = yesNo.ToJson();
            Console.WriteLine(json);
            Assert.AreEqual(json, "null");

            var yesNoDef = (YesOrNoDefault)0;
            json = yesNoDef.ToJson();
            Console.WriteLine(json);
            Assert.AreEqual(json, "NO".ToJson());
        }


        [Test]
        public void CouldSerializeNumberPropertyAsString() {
            var t = new TypeWithNumberSerializedAsString();
            t.NormalInt = 123;
            t.AsString = 456;
            var j = t.ToJson();
            Console.WriteLine(j);
            var t2 = j.FromJson<TypeWithNumberDeSerializedAsString>();
            Assert.AreEqual("456", t2.AsString);
            var t1 = j.FromJson<TypeWithNumberSerializedAsString>();
            Assert.AreEqual(456, t1.AsString);
        }


        [Test]
        public void CouldSerializeDateTimePropertyAsString() {
            var t = new TypeWithDateTimeSerializedAsString();
            t.AsString = DateTime.Now;
            var j = t.ToJson();
            Console.WriteLine(j);
            var t2 = j.FromJson<TypeWithDateTimeDeSerializedAsString>();
            Assert.AreEqual(t.AsString.Value.ToString("hhmmss"), t2.AsString);

        }

    }
}