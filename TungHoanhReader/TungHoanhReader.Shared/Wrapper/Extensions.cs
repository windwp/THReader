using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;

namespace TungHoanhReader.Wrapper
{
    public static class Extensions
    {

        public static string ToLsbString(this TagTruyen enumerationValue)
        {
            Type type = enumerationValue.GetType();

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetRuntimeFields();
            if (memberInfo != null && memberInfo.First(o => o.Name == enumerationValue.ToString()) != null)
            {
                var attr = memberInfo.First(o => o.Name == enumerationValue.ToString()).GetCustomAttribute(typeof(LSBStringValue), false) as LSBStringValue;
                if (attr != null) return attr.Value;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        public static string ToTruyenConvertString(this TagTruyen enumerationValue)
        {
            Type type = enumerationValue.GetType();

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetRuntimeFields();
            if (memberInfo != null && memberInfo.First(o => o.Name == enumerationValue.ToString()) != null)
            {
                var attr = memberInfo.First(o => o.Name == enumerationValue.ToString()).GetCustomAttribute(typeof(TruyenConvertStringValue), false) as TruyenConvertStringValue;
                if (attr != null) return attr.Value;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        public static string ToLsbHienThiString(this TagTruyen enumerationValue)
        {
            Type type = enumerationValue.GetType();

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetRuntimeFields();
            if (memberInfo != null && memberInfo.First(o => o.Name == enumerationValue.ToString()) != null)
            {
                var attr = memberInfo.First(o => o.Name == enumerationValue.ToString()).GetCustomAttribute(typeof(HienThiStringValue), false) as HienThiStringValue;
                if (attr != null) return attr.Value;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        /// <summary>
        /// Extension method for xpath support selectNodes with simple xpath query on ly support xpat with / and xpath [number]
        /// example xpath /html/body/div[2]/div[2]/di
        /// </summary>
        /// <param name="dom"></param>
        /// <param name="xpathquery"></param>
        /// <returns></returns>
        public static List<HtmlNode> SelectNodes(this HtmlNode dom, string xpathquery)
        {
            if (xpathquery.Contains(":"))
            {
                throw new InvalidDataException("This method don't support this query");
            }
            HtmlNode currentNode = dom;
            if (xpathquery.StartsWith("/"))
            {
                xpathquery = xpathquery.Substring(1, xpathquery.Length - 1);
                currentNode = currentNode.OwnerDocument.DocumentNode;
            }
            else if (xpathquery.StartsWith("./"))
            {
                xpathquery = xpathquery.Substring(2, xpathquery.Length - 2);
            }
            var listQuery = xpathquery.Split('/');
            List<HtmlNode> listResultNode = null;
            for (int i = 0; i < listQuery.Length; i++)
            {
                listResultNode = null;
                var currentQuery = listQuery[i];
                if (string.IsNullOrEmpty(currentQuery))
                {
                    // process // query for one node only 
                    // find first child node for next node
                    //lay node ke tiep ra tim kiem
                    var nextquery = listQuery[++i];
                    foreach (var iNode in currentNode.Descendants())
                    {
                        var tNode = iNode.SelectNodes(nextquery);
                        listResultNode = tNode;
                        if (tNode != null && tNode.Count >= 1)
                        {
                            currentNode = tNode[0];
                            break;
                        }
                    }
                    if (currentNode == null) return null;

                }
                else if (currentQuery.Contains("[@"))
                {
                    currentQuery = currentQuery.Replace("\"", "'");
                    var nodeNameFound = currentQuery.Substring(0, currentQuery.IndexOf("["));
                    var atributeName = currentQuery.Substring(currentQuery.IndexOf("@") + 1, currentQuery.IndexOf("=") - currentQuery.IndexOf("@") - 1);
                    var atributeValue = currentQuery.Substring(currentQuery.IndexOf("'") + 1, currentQuery.LastIndexOf("'") - currentQuery.IndexOf("'") - 1);
                    foreach (var iNode in currentNode.Descendants(nodeNameFound))
                    {
                        if (iNode.GetAttributeValue(atributeName, "") != null &&
                            iNode.GetAttributeValue(atributeName, "") == atributeValue)
                        {
                            currentNode = iNode;
                            break;
                        }
                    }
                    if (currentNode == null || currentNode.Name != nodeNameFound) return null;
                    if (i >= listQuery.Length - 1)
                    {
                        var result = new List<HtmlNode>();
                        result.Add(currentNode);
                        return result;
                    }
                }
                else if (currentQuery.Contains("["))
                {
                    //neu' k0 co' node tra? ve null;

                    // lay' thu' tu. cua? node
                    var numberStr = currentQuery.Substring(currentQuery.IndexOf("[") + 1, currentQuery.IndexOf("]") - currentQuery.IndexOf("[") - 1);
                    var number = 0;
                    int.TryParse(numberStr, out number);
                    currentQuery = currentQuery.Substring(0, currentQuery.IndexOf("["));

                    //if (dom.ChildNodes.Where(o => o.Name == currentQuery) == null) return null;

                    var listNode = currentNode.ChildNodes.Where(o => o.Name == currentQuery);
                    var htmlNodes = listNode.ToList();
                    if (htmlNodes.Count() > number - 1)
                    {
                        currentNode = htmlNodes[number - 1];
                    }
                    else
                    {
                        return null;
                    }
                    if (i >= listQuery.Length - 1)
                    {
                        var result = new List<HtmlNode>();
                        result.Add(currentNode);
                        return result;
                    }
                }
                else
                {
                    //neu' nhu tim` kiem toi' hang cuoi cung roi`
                    if (i >= listQuery.Length - 1)
                    {
                        return currentNode.ChildNodes.Where(o => o.Name == currentQuery).ToList();
                    }
                    //neu nhu chi thay' 1 nut' tuong ung' thi` gan' nu't hien tai. la` nut' do'
                    if (dom.ChildNodes.FirstOrDefault(o => o.Name == currentQuery) != null)
                    {
                        currentNode = dom.ChildNodes.First(o => o.Name == currentQuery);
                    }
                    else
                    {
                        //ko co' thi` tra? ve null
                        return null;
                    }

                }
            }

            return listResultNode;
        }
        /// <summary>
        /// Extension method for xpath support selectNodes with simple xpath query on ly support xpat with / and xpath [number]
        /// example xpath /html/body/div[2]/div[2]/di
        /// </summary>
        /// <param name="dom"></param>
        /// <param name="xpathquery"></param>
        /// <returns></returns>
        public static HtmlNode SelectSingleNode(this HtmlNode dom, string xpathquery)
        {
            if (xpathquery.Contains(":"))
            {
                throw new InvalidDataException("This method don't support this query");
            }

            var result = dom.SelectNodes(xpathquery);
            if (result == null || result.Count != 1) return null;
            if (result.Count == 1)
            {
                return result[0];
            }
            return null;
        }

    }
}