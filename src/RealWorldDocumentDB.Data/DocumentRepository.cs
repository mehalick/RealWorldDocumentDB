using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace RealWorldDocumentDB.Data
{
    public class DocumentRepository<T> where T : class
    {
        private const string DatabaseId = "MyDatabase";
        private const string CollectionId = "MyCollection";

        private readonly IDocumentClient _documentClient;
        private readonly Uri _collectionUri;

        public DocumentRepository(IDocumentClient documentClient)
        {
            _documentClient = documentClient;
            _collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
        }

        private static Uri GetDocumentUri(string id)
        {
            return UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
        }

        public async Task<T> GetById(string id)
        {
            try
            {
                var document = await _documentClient.ReadDocumentAsync(GetDocumentUri(id));

                return (T)(dynamic)document.Resource;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<IReadOnlyCollection<T>> GetItems(Expression<Func<T, bool>> predicate)
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1 };

            var query = _documentClient.CreateDocumentQuery<T>(_collectionUri, feedOptions)
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T> CreateItem(T item)
        {
            var document = await _documentClient.CreateDocumentAsync(_collectionUri, item);

            return (T)(dynamic)document.Resource;
        }

        public async Task<Document> UpdateItem(string id, T item)
        {
            return await _documentClient.ReplaceDocumentAsync(GetDocumentUri(id), item);
        }

        public async Task DeleteItem(string id)
        {
            await _documentClient.DeleteDocumentAsync(GetDocumentUri(id));
        }
    }
}
