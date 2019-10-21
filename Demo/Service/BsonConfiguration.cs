using System;
using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Service
{
    public static class BsonConfiguration
    {
        private static readonly LazyAction lazy = new LazyAction(() =>
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
        });

        public static void Configure() => lazy.Invoke();

        private class LazyAction : Lazy<object>
        {
            public LazyAction(Action action) : base(() => Invoke(action))
            {
            }

            public void Invoke()
            {
                var _ = Value;
            }

            private static object Invoke(Action action)
            {
                action.Invoke();
                return null;
            }
        }
    }
}