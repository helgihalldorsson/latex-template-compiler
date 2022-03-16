using NUnit.Framework;
using FluentAssertions;
using template.data.Nodes;
using System.Threading.Tasks;

namespace template.data.tests
{
    public class TemplateDataTests
    {
        #region Deserialize 

        [Test]
        public void TestDeserializeAsBoolNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": true }";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<BoolNode>();
        }

        [Test]
        public void TestDeserializeAsDoubleNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": 123.45 }";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<DoubleNode>();            
        }

        [Test]
        public void TestDeserializeAsIntNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": 123 }";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<IntNode>();
        }

        [Test]
        public void TestDeserializeAsStringNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": \"abc\"}";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<StringNode>();
        }

        [Test]
        public void TestDeserializeAsListNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": [ 1, 2, 3 ] }";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<ListNode>();
        }

        [Test]
        public void TestDeserializeAsDictionaryNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"alpha\": 1, \"beta\": \"two\" } }";

            // Act
            TemplateData result = TemplateData.Deserialize(json);

            // Assert
            result.Should().NotBeNull();
            result.data.Should().BeOfType<DictionaryNode>();
        }

        [Test]
        public void TestDeserializeInvalidJson()
        {
            // Arrange
            string json = "{\"data\": {";

            // Act
            Task Act() => Task.Run(() => TemplateData.Deserialize(json));

            // Assert
            Assert.That(Act, Throws.TypeOf<Newtonsoft.Json.JsonReaderException>());
        }
        #endregion

        #region FillTemplate 
        [Test]
        public void TestFillTemplateBoolNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": true } }";            
            string template = "Here is <:value:>";
            string expected = "Here is True";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateDoubleNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": 12.34 } }";
            string template = "Here is <:value:>";
            string expected = "Here is 12.34";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateIntNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": 42 } }";
            string template = "Here is <:value:>";
            string expected = "Here is 42";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateStringNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": \"a value\" } }";
            string template = "Here is <:value:>";
            string expected = "Here is a value";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateListNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": [ \"alpha\", \"beta\", \"gamma\" ] } }";
            string template = "Here is <:value::><> - <><:value:><::value:>.";
            string expected = "Here is alpha - beta - gamma.";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateDictionaryNodeSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"value\": { \"alpha\": 1, \"beta\": 3 } } }";
            string template = "Here is <:value:alpha:>+<:value:beta:>";
            string expected = "Here is 1+3";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateListOfDictionaryNodesSuccess()
        {
            // Arrange
            string json = "{\"data\": { \"list\": [{ \"alpha\": 1, \"beta\": 2 }, { \"alpha\": 3, \"beta\": 4 }, { \"alpha\": 5, \"beta\": 6 }] } }";
            string template = "Here is <:list::><>, <><:list:alpha:> + <:list:beta:><::list:>.";
            string expected = "Here is 1 + 2, 3 + 4, 5 + 6.";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }
        #endregion

        #region FillTemplate Settings
        [Test]
        public void TestFillTemplateListWithIgnoreToggledOnSuccess()
        {
            // Arrange
            string json = "{\"settings\": { \"ignoreEnabled\": true }, \"data\": { \"list\": [{ \"alpha\": 1 }, { \"alpha\": 2 }, { \"alpha\": 3, \"ignore\": true }, { \"alpha\": 4 }] } }";
            string template = "Here is <:list::><>, <><:list:alpha:><::list:>.";
            string expected = "Here is 1, 2, 4.";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateListWithIgnoreToggledOffSuccess()
        {
            // Arrange
            string json = "{\"settings\": { \"ignoreEnabled\": false }, \"data\": { \"list\": [{ \"alpha\": 1 }, { \"alpha\": 2 }, { \"alpha\": 3, \"ignore\": true }, { \"alpha\": 4 }] } }";
            string template = "Here is <:list::><>, <><:list:alpha:><::list:>.";
            string expected = "Here is 1, 2, 3, 4.";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Test]
        public void TestFillTemplateSelectedLanguageSuccess()
        {
            // Arrange
            string json = "{\"settings\": { \"supportedLanguages\": [ \"is\", \"en\" ] }, \"data\": { \"value\": { \"is\": \"Halló\", \"en\": \"Hello\" } } }";
            string selectedLanguage = "en";
            string template = "Say '<:value:>'.";
            string expected = "Say 'Hello'.";
            TemplateData data = TemplateData.Deserialize(json);

            // Act
            string result = data.FillTemplate(template, selectedLanguage);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }
        #endregion
    }
}