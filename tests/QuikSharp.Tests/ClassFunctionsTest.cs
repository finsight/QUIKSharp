using System;
using NUnit.Framework;

namespace QuikSharp.Tests {
    [TestFixture]
    public class ClassFunctionsTest {
        public ClassFunctionsTest() { _isQuik = _q.Debug.IsQuik().Result; }
        private Quik _q = new Quik();
        private bool _isQuik;

        [Test]
        public void GetClassesList() {
            Console.WriteLine("GetClassesList: "
                + String.Join(",", _q.Class.GetClassesList().Result));
        }

        [Test]
        public void GetClassInfo() {
            var list = _q.Class.GetClassesList().Result;
            foreach (var s in list) {
                Console.WriteLine("GetClassInfo for " + s + ": "
                + String.Join(",", _q.Class.GetClassInfo(s).Result));
            }
        }



        [Test]
        public void GetClassSecurities() {
            var list = _q.Class.GetClassesList().Result;
            foreach (var s in list) {
                Console.WriteLine("GetClassSecurities for " + s + ": "
                + String.Join(",", _q.Class.GetClassSecurities(s).Result));
            }
        }

        [Test]
        public void GetSecurityInfo() {
            Console.WriteLine("GetSecurityInfo for RIM5: "
            + String.Join(",", _q.Class.GetSecurityInfo("SPBFUT", "RIM5").Result.ToJson()));

            Console.WriteLine("GetSecurityInfo for LKOH: "
            + String.Join(",", _q.Class.GetSecurityInfo("TQBR", "LKOH").Result.ToJson()));
        }

    }
}
