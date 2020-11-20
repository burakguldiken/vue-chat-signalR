<template>
  <v-container>
    <v-row>
      <v-col cols="2" sm="12" md="2" xs="12">
        <Slide isOpen>
          <v-list mx-auto flat style="background: transparent; width: 250px">
            <v-list-item>
              <v-list-item-title>
                <v-icon medium color="green darken-2" class="mr-2">
                  mdi-message-text
                </v-icon>
                <span style="font-size: 20px" class="white--text"
                  >Chat Room</span
                >
              </v-list-item-title>
            </v-list-item>

            <v-list-group :value="true" class="ml-5">
              <template v-slot:activator>
                <v-icon class="mr-2" color="primary">mdi-account-circle</v-icon>
                <v-list-item-title class="white--text">Users</v-list-item-title>
              </template>

              <v-list-item
                class="ml-5"
                v-for="(item, index) in users" :key="index"
                link
              >
                <v-avatar class="mr-2" size="25px">
                  <img
                    alt="Avatar"
                    src="https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
                  />
                </v-avatar>
                <v-list-item-title
                  class="white--text"
                  v-text="item.name"
                ></v-list-item-title>
              </v-list-item>
            </v-list-group>
          </v-list>
        </Slide>
      </v-col>
    </v-row>
    <v-row class="mt-8" justify="end">
      <v-col cols="12" sm="12" md="10" xs="12">
        <v-toolbar color="primary" dark>
          <v-btn @click="openDialog" small class="mx-2" fab dark color="red">
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>

          <v-dialog v-model="dialog" max-width="290">
            <v-card>
              <v-card-title class="headline">
                Join Room
              </v-card-title>

              <v-card-text>
                <v-col cols="12">
                  <v-text-field
                    v-model="name"
                    counter
                    maxlength="20"
                    hint="Username must be 20 characters maximum"
                    label="Your username"
                  ></v-text-field>
                </v-col>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>

                <v-btn color="green darken-1" text @click="joinRoom">
                  Join
                </v-btn>

              </v-card-actions>
            </v-card>
          </v-dialog>

          <v-toolbar-title>
            <span>Vue Chat</span>
          </v-toolbar-title>

          <v-spacer></v-spacer>

          <v-btn icon>
            <v-icon>mdi-magnify</v-icon>
          </v-btn>

          <v-btn icon>
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </v-toolbar>

        <v-card class="card-wrapper elevation-12" flat>
          <v-card-text class="card-text-wrapper">
            <v-col cols="12" sm="12" md="3" xs="12">
              <v-row v-for="(item, index) in Messages" :key="index">
                <v-avatar class="ma-2" size="25px">
                  <img
                    alt="Avatar"
                    src="https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
                  />
                </v-avatar>

                <v-textarea
                  v-model="item.message"
                  dense
                  auto-grow
                  outlined
                  rows="1"
                  row-height="15"
                  readonly
                  rounded
                ></v-textarea>
              </v-row>
            </v-col>
          </v-card-text>

          <v-card-actions class="card-action-wrapper">
            <v-row>
              <v-col cols="12">
                <v-divider></v-divider>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="messageContent"
                  class="ml-6 mr-12"
                  counter
                  maxlength="100"
                  hint="Message must be 100 characters maximum"
                  label="Send Message..."
                  append-icon="mdi-send"
                  @click:append="sendMessage"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { Slide } from "vue-burger-menu";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
export default {
  components: {
    Slide,
  },
  data() {
    return {
      messageContent: "",
      dialog: false,
      name: "",
      connection: null,
      users: [{name: "Test User"}],
      Messages: [{ message: "Hello" }],
    };
  },

  methods: {
    sendMessage() {
      this.connection.invoke("SendMessage",this.messageContent).catch(function (err) {
        return console.error(err);
      });
    },

    openDialog() {
      this.dialog = true;
    },

    joinRoom() {
      this.dialog = false;
      this.connection.invoke("JoinRoom",this.name).catch(function (err) {
        return console.error(err);
      });
    },

    listen() {
      window.console.log("Listen Started");

      if (this.connection.state !== HubConnectionState.Connected) {
        this.connect().finally(() => {
          this.listen();
          return;
        });
      }

      this.connection.on("NewConnection", (res) => {
        console.log(res);
      });

      this.connection.on("JoinRoom", (res) => {
        var userObj = {
          name: res
        };
        this.users.push(userObj);
      });

      this.connection.on("SendMessage",(res) => {
        var messageObj = {
          message: res
        };
        this.Messages.push(messageObj);
        console.log(res);
      })
    },

  },

  created() {
    if (this.connection === null) {
      this.connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5200/chathub")
        .build();
    }
    this.connection
      .start()
      .then(() => {
        window.console.log("Connection Success");
        this.listen();
      })
      .catch((err) => {
        window.console.log(`Connection Error ${err}`);
      });

    this.connection.onclose(() => {
      window.console.log("Connection Destroy");
    });
  },
};
</script>

<style scoped>
@import '../css/Chat.css';
</style>