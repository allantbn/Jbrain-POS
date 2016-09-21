using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PointOfSale
{
    [TestClass]
    public class SellOneItemTest
    {
        private Display display;
        private Sale sale;

        [TestInitialize]
        public void TestInitialize()
        {
            display = new Display();
            sale = new Sale(display, new Dictionary<String, String>()
            {
                {"12345", "$7.95"},
                {"23456", "$12.50"}
            });
        }

        [TestMethod]
        public void ProductFound()
        {
            sale.OnBarcode("12345");
            Assert.AreEqual("$7.95", display.GetText());
        }

        [TestMethod]
        public void AnotherProductFound()
        {
            sale.OnBarcode("23456");
            Assert.AreEqual("$12.50", display.GetText());
        }

        [TestMethod]
        public void ProductNotFound()
        {
            sale.OnBarcode("99999");
            Assert.AreEqual("Product not found for 99999", display.GetText());
        }

        [TestMethod]
        public void EmtpyBarcode()
        {
            Sale sale = new Sale(display, null);
            sale.OnBarcode("");
            Assert.AreEqual("Scanning error: empty barcode", display.GetText());
        }
    }
  
    internal class Sale
    {
        private readonly Display _display;
        private readonly Dictionary<string, string> _pricesByBarcode;

        public Sale(Display dislpay, Dictionary<string, string> pricesByBarcode)
        {
            this._display = dislpay;
            this._pricesByBarcode = pricesByBarcode;
        }

        public void OnBarcode(string barcode)
        {
            if ("".Equals(barcode))
            {
                this._display.SetText("Scanning error: empty barcode");
                return;
            }

            if (_pricesByBarcode.ContainsKey(barcode))
            {
                this._display.SetText(_pricesByBarcode[barcode]);
            }
            else
            {
                this._display.SetText("Product not found for " + barcode);
            }
            
        }
    }

    internal class Display
    {
        private string _text;

        public string GetText()
        {
            return _text;
        }

        public void SetText(string text)
        {
            this._text = text;
        }
    }
}
