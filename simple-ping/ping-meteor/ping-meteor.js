if (Meteor.isClient) {

  Template.pong.helpers({
    lastPong: function(){
      return Session.get("lastPong");
    }
  });

  Template.ping.events({
    'click .client-ping': function () {
      console.log('client ping sent');
      MassTransit.publish('PingMassTransit:Ping', {});
    },
    'click .server-ping': function () {
      Meteor.call('ping');
    }
  });

  MassTransit.inbound.find({}).observe({
    added: function(doc) {
      console.log('client pong received')
      console.log(doc);

      Session.set("lastPong", doc.message.dateTime);

      MassTransit.inbound.remove(doc._id);
    }
  });

}

if (Meteor.isServer) {

  MassTransit.init({host: 'localhost', queueName: 'ping-meteor'});

  MassTransit.onConnected = function() {
    MassTransit.bind('PingMassTransit:Pong');
  };

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
      MassTransit.publish('PingMassTransit:Ping', {});
    }
  });

}
