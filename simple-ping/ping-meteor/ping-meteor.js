if (Meteor.isClient) {

  Template.pong.helpers({
    lastPing: function(){
      return Session.get('lastPing');
    },
    lastPong: function(){
      return Session.get('lastPong');
    },
    lastLatency: function(){
      return Session.get('lastLatency');
    }
  });

  Template.ping.events({
    'click .client-ping': function () {
      console.log('client ping sent');

      var lastPingDate = new Date();

      Session.set('lastPing', lastPingDate.toISOString());

      MassTransit.publish('PingMassTransit:Ping', {
        SomeInteger: 1234,
        SomeDecimal: 2345.6,
        SomeString: 'Hello World!',
        SomeDate: lastPingDate
      });

    },
    'click .server-ping': function () {
      Meteor.call('ping');
    }
  });

  MassTransit.inbound.find({}).observe({
    added: function(doc) {
      console.log('client pong received')
      console.log(doc);

      var pingDate = new Date(doc.message.someDate);
      var pongDate = new Date();
      var latency = pongDate - pingDate;

      Session.set('lastPong', pongDate.toISOString());
      Session.set('lastLatency', latency);

      MassTransit.inbound.remove(doc._id);
    }
  });

}

if (Meteor.isServer) {
  MassTransit.init({
    connection: {
      host: 'localhost'
    },
    queue: {
      name: 'ping-meteor'
    },
    exchange: {
      type: 'fanout'
    }
  });

  MassTransit.bind('PingMassTransit:Pong');

  MassTransit.inbound.find({}).observe({
    added: function(doc) {
      console.log('server pong received')
      console.log(doc);

      MassTransit.inbound.remove(doc._id);
    }
  });

  Meteor.methods({
    ping: function() {
      console.log('server ping sent');
      MassTransit.publish('PingMassTransit:Ping', {
        SomeInteger: 1234,
        SomeDecimal: 2345.6,
        SomeString: 'Hello World!',
        SomeDate: new Date()
      });
    }
  });

}
