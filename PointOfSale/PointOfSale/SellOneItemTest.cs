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
        [TestMethod]
        public void ProductFound()
        {
            Display display = new Display();
            Sale sale = new Sale(display, new Dictionary<String, String>()
            {
                {"12345", "$7.95"},
                {"23456", "$12.50"}
            });
            sale.OnBarcode("12345");
            Assert.AreEqual("$7.95", display.GetText());
        }

        [TestMethod]
        public void AnotherProductFound()
        {
            Display display = new Display();
            Sale sale = new Sale(display, new Dictionary<String, String>()
            {
                {"12345", "$7.95"},
                {"23456", "$12.50"}
            });
            sale.OnBarcode("23456");
            Assert.AreEqual("$12.50", display.GetText());
        }

        [TestMethod]
        public void ProductNotFound()
        {
            Display display = new Display();
            Sale sale = new Sale(display, new Dictionary<String, String>()
            {
                {"12345", "$7.95"},
                {"23456", "$12.50"}
            });
            sale.OnBarcode("99999");
            Assert.AreEqual("Product not found for 99999", display.GetText());
        }

        [TestMethod]
        public void EmtpyBarcode()
        {
            Display display = new Display();
            Sale sale = new Sale(display, new Dictionary<String, String>()
            {
                {"12345", "$7.95"},
                {"23456", "$12.50"}
            });
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
            }
            else
            {
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
