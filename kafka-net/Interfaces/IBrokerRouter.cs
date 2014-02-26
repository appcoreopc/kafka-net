﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaNet
{
    public interface IBrokerRouter : IDisposable
    {
        /// <summary>
        /// Get list of default broker connections.  This list is provided by the class constructor options and is used to query for metadata.
        /// </summary>
        List<IKafkaConnection> DefaultBrokers { get; }

        /// <summary>
        /// Select a broker for a specific topic and partitionId.
        /// </summary>
        /// <param name="topic">The topic name to select a broker for.</param>
        /// <param name="partitionId">The exact partition to select a broker for.</param>
        /// <returns>A broker route for the given partition of the given topic.</returns>
        /// <remarks>
        /// This function does not use any selector criteria.  If the given partitionId does not exist an exception will be thrown.
        /// </remarks>
        /// <exception cref="InvalidTopicMetadataException">Thrown if the returned metadata for the given topic is invalid or missing.</exception>
        /// <exception cref="InvalidPartitionException">Thrown if the give partitionId does not exist for the given topic.</exception>
        /// <exception cref="ServerUnreachableException">Thrown if none of the Default Brokers can be contacted.</exception>
        Task<BrokerRoute> SelectBrokerRouteAsync(string topic, int partitionId);

        /// <summary>
        /// Select a broker for a given topic using the IPartitionSelector function.
        /// </summary>
        /// <param name="topic">The topic to retreive a broker route for.</param>
        /// <param name="key">The key used by the IPartitionSelector to collate to a consistent partition. Null value means key will be ignored in selection process.</param>
        /// <returns>A broker route for the given topic.</returns>
        /// <exception cref="InvalidTopicMetadataException">Thrown if the returned metadata for the given topic is invalid or missing.</exception>
        /// <exception cref="ServerUnreachableException">Thrown if none of the Default Brokers can be contacted.</exception>
        Task<BrokerRoute> SelectBrokerRouteAsync(string topic, string key = null);

        /// <summary>
        /// Returns Topic metadata for each topic requested. 
        /// </summary>
        /// <param name="topics">Collection of topids to request metadata for.</param>
        /// <returns>List of Topics as provided by Kafka.</returns>
        /// <remarks>The topic metadata will by default check the cache first and then request metadata from the server if it does not exist in cache.</remarks>
        Task<List<Topic>> GetTopicMetadataAsync(params string[] topics);
    }
}