using ChitChat.Models;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using ChitChat.Helper.Extensions;

namespace ChitChat.Helper.Converters
{
    class TextAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            UserModel currentUser = values[0] as UserModel;
            string sender = values[1] as string;

            if (currentUser.DisplayName != sender)
            {
                if (IsFlowDocument(parameter as string))
                {
                    var flowDocument = SetFlowDocumentAlignment
                        ((values[2] as FlowDocument),
                        TextAlignment.Left);

                    SetFlowDocumentAlignment((values[3] as FlowDocumentScrollViewer),
                    HorizontalAlignment.Left);

                    SetParagraphAlignment(flowDocument, TextAlignment.Left);
                    TryDisconnectFlowDocument(flowDocument);
                    return flowDocument;
                }
                else
                {
                    return TextAlignment.Left;
                }
            }
            if (IsFlowDocument(parameter as string))
            {
                var document = (values[2] as FlowDocument);
                var flowDocument = SetFlowDocumentAlignment(document, TextAlignment.Right);

                SetFlowDocumentAlignment((values[3] as FlowDocumentScrollViewer), 
                    HorizontalAlignment.Right);

                SetParagraphAlignment(flowDocument, TextAlignment.Right);
                TryDisconnectFlowDocument(flowDocument);
                return flowDocument;
            }
            return TextAlignment.Right;
        }

        private bool IsFlowDocument(string parameter)
        {
            return (parameter as string) == "FlowDocument";
        }

        private FlowDocument SetFlowDocumentAlignment<TDocument, TAlignment>(TDocument document, TAlignment alingment)
        {
            FlowDocument flowDocument;
            if (document.GetType() == typeof(FlowDocument)) 
            {
                flowDocument = (document as FlowDocument);
                flowDocument.TextAlignment = (TextAlignment)(object)alingment;
            }
            else
            {
                (document as FlowDocumentScrollViewer).HorizontalAlignment = (HorizontalAlignment)(object)alingment;
                return null;
            }
            return flowDocument;
        }

        /// <summary>
        /// If document has a parent, disconnect it before returning.
        /// </summary>
        /// <param name="flowDocument"></param>
        private void TryDisconnectFlowDocument(FlowDocument flowDocument)
        {
            if (flowDocument.Parent != null)
            {
                var parent = (flowDocument.Parent as FlowDocumentScrollViewer);
                parent.Document = null;
            }
        }

        private void SetParagraphAlignment(FlowDocument flowDocument, TextAlignment alignment)
        {
            foreach (Paragraph paragraph in flowDocument.Blocks.ToList())
            {
                paragraph.TextAlignment = alignment;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
