using CardinalWebApplication.Models;
using CardinalWebApplication.Services;
using System;
using Xunit;

namespace CardinalWebApplicationXUnitTest
{
    public class HexagonalEquilateralScaleTest
    {
        [Fact]
        public void TestPolygonFromDelimited()
        {
            HexagonalEquilateralScale hex = new HexagonalEquilateralScale();
            hex.Initialize(0, 0, 1);
            string delimited = "1x20x20";
            Polygon poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(6, poly.Positions.Count);
            Assert.Equal(delimited, poly.Tag.ToString());

            delimited = "3x15x10";
            poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(delimited, poly.Tag.ToString());

            delimited = "3x150x109";
            poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(delimited, poly.Tag.ToString());

            delimited = "3x-15x10";
            poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(delimited, poly.Tag.ToString());

            delimited = "3x15x-10";
            poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(delimited, poly.Tag.ToString());

            delimited = "3x-151x-13";
            poly = hex.PolygonFromDelimited(delimited);
            Assert.Equal(delimited, poly.Tag.ToString());

        }
    }
}
