﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace DotnetSpider.Extraction.ExcelExpression.HapCss.Selectors
{
    internal class IdSelector : CssSelector
    {
        public override string Token
        {
            get { return "#"; }
        }

        protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes)
        {
            foreach (var node in currentNodes)
            {
                if (node.Id.Equals(Selector, StringComparison.InvariantCultureIgnoreCase))
                    return new[] { node };
            }

            return new HtmlNode[0];
        }
    }
}
