using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PointOfSale
{
    [TestClass]
    public class SellOneItemTest
    {
        private Display _display;
        private Sale _sale;

        [TestInitialize]
        public void TestInitialize()
        {
            _display = new Display();
            _sale = new Sale(_display, new Catalog(new Dictionary<string, string>()
                    {
                        { "12345", "$7.95"},
                        {"23456", "$12.50"}
                    }
                    )
                );
        }

        [TestMethod]
        public void ProductFound()
        {
            _sale.OnBarcode("12345");
            Assert.AreEqual("$7.95", _display.GetText());
        }

        [TestMethod]
        public void AnotherProductFound()
        {
            _sale.OnBarcode("23456");
            Assert.AreEqual("$12.50", _display.GetText());
        }

        [TestMethod]
        public void ProductNotFound()
        {
            _sale.OnBarcode("99999");
            Assert.AreEqual("Product not found for 99999", _display.GetText());
        }

        [TestMethod]
        public void EmtpyBarcode()
        {
            var sale = new Sale(_display, new Catalog(null));
            sale.OnBarcode("");
            Assert.AreEqual("Scanning error: empty barcode", _display.GetText());
        }
    }

    internal class Sale
    {
        private readonly Display _display;
        private readonly Catalog _catalog;

        public Sale(Display dislpay, Catalog catalog)
        {
            _catalog = catalog;
            this._display = dislpay;
        }

        public void OnBarcode(string barcode)
        {
            if ("".Equals(barcode))
            {
                _display.DisplayEmptyBarCodeMessage();
                return;
            }

            string priceAsText = _catalog.FindPrice(barcode);
            if (priceAsText == null)
            {
                _display.DisplayProductNotFoundMessage(barcode);
            }
            else
            {
                _display.DisplayePrice(priceAsText);
            }
        }
    }

    internal class Catalog
    {
        private readonly Dictionary<string, string> _pricesByBarcode;

        public Catalog(Dictionary<string, string> pricesByBarcode)
        {
            _pricesByBarcode = pricesByBarcode;
        }

        public string FindPrice(string barcode)
        {
            return _pricesByBarcode.ContainsKey(barcode) ? _pricesByBarcode[barcode] : null;
        }
    }

    internal class Display
    {
        private string _text;

        public string GetText()
        {
            return _text;
        }

        public void DisplayePrice(string priceAsText)
        {
            this._text = priceAsText;
        }

        public void DisplayProductNotFoundMessage(string barcode)
        {
            this._text = "Product not found for " + barcode;
        }

        public void DisplayEmptyBarCodeMessage()
        {
            this._text = "Scanning error: empty barcode";
        }
    }
}
