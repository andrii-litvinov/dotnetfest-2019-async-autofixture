using Contracts.Events;
using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Service
{
    public static class BsonConfiguration
    {
        public static void Configure()
        {
            ConventionRegistry.Register("conventions", new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            }, type => true);

            BsonClassMap.RegisterClassMap<AggregateRoot>(map =>
            {
                map.AutoMap();
                map
                    .MapMember(root => root.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                map.UnmapMember(root => root.Events);
                map.MapMember(root => root.Outbox).SetElementName("_outbox");
            });

            BsonClassMap.RegisterClassMap<Envelope>(map =>
            {
                map.AutoMap();
                map.SetDiscriminatorIsRequired(true);
            });
        }
    }
}