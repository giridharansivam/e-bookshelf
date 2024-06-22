using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace _301335487_Paramasivam__Lab_2
{
    internal class DynamoDBHelper
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA3FLD4MA7I4FSAAXY", "fufLScC0oTmZD4iY8r+VFpCQLZvVJt3LEWRsGI6y", Amazon.RegionEndpoint.USEast1 
);

        public static async Task CreateUserCredentialsTableAsync()
        {
            var createTableRequest = new CreateTableRequest
            {
                TableName = "UserCredentials",
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition { AttributeName = "Username", AttributeType = "S" }
            },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement { AttributeName = "Username", KeyType = "HASH" }
            },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            await client.CreateTableAsync(createTableRequest);
        }
        public static async Task InsertUserCredentialsAsync(string Username, string password)
        {
            var request = new PutItemRequest
            {
                TableName = "UserCredentials",
                Item = new Dictionary<string, AttributeValue>
        {
            { "Username", new AttributeValue { S = Username } },
            { "Password", new AttributeValue { S = password } }
        }
            };

            await client.PutItemAsync(request);
        }
        public static async Task<bool> ValidateUserAsync(string Username, string password)
        {
            var request = new GetItemRequest
            {
                TableName = "UserCredentials",
                Key = new Dictionary<string, AttributeValue>
        {
            { "Username", new AttributeValue { S = Username } }
        }
            };

            var response = await client.GetItemAsync(request);
            if (response.Item != null && response.Item.ContainsKey("Password"))
            {
                return response.Item["Password"].S == password;
            }
            return false;
        }
        public static async Task<List<Book>> GetBooksForUserAsync(string Username)
        {
            var request = new QueryRequest
            {
                TableName = "Bookshelf",
                KeyConditionExpression = "Username = :Username",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        {
            { ":Username", new AttributeValue { S = Username } }
        }
            };

            var response = await client.QueryAsync(request);

            var books = new List<Book>();

            foreach (var item in response.Items)
            {
                // Assuming you have attributes like Title, Author, etc.
                var book = new Book
                {
                    Title = item["Title"].S,
                    Author = item["Author"].S,
                    s3Url = item["s3Url"].S,
                };

                books.Add(book);
            }

            return books;
        }
        public static async Task UpdateBookCurrentPageAsync(string username, string bookID, int currentPage)
        {
            var request = new UpdateItemRequest
            {
                TableName = "Bookshelf",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Username", new AttributeValue { S = username } },
                    { "BookID", new AttributeValue { S = bookID } } // Assuming BookID is a string
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "CurrentPage", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { N = currentPage.ToString() } } }
                }
            };

            await client.UpdateItemAsync(request);
        }


    }
}
