<template>
  <v-form v-model="valid">
    <v-text-field v-model="alert.name" label="Name" :required="true"></v-text-field>
    <v-textarea v-model="alert.description" label="Description" required auto-grow></v-textarea>
    <v-text-field v-model="alert.value" label="Value" required></v-text-field>
    <v-text-field v-model="alert.threshold" label="Threshold" type="number"></v-text-field>
    <v-switch v-model="alert.hasConstantValue" class="ma-4" label="Is Value Constant?"></v-switch>
    <v-select
      v-model="alert.generalRuleId"
      :items="selectableCustomAttributeRules"
      item-text="name"
      item-value="id"
      label="Rule on which to alert"
    ></v-select>
    <v-row>
      <v-col cols="12" sm="4" md="4" lg="4">
        <v-text-field
          v-model="alert.timespan"
          label="Seconds"
          type="number"
          :rules="[rules.required]"
        ></v-text-field>
      </v-col>
    </v-row>
    <v-btn color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "alert-form",
  data: () => {
    return {
      rules: {
        required: value => !!value || "Required.",
        time: value => {
          const parsed = parseInt(value);
          if (parsed >= 0 && parsed <= 60) {
            return true;
          } else {
            return "Invalid value";
          }
        }
      },
      valid: true,
      alertBeforeEdit: {
        id: 0,
        name: "",
        description: "",
        threshold: 0,
        timespan: 0,
        value: "",
        componentId: 0,
        generalRuleId: 0
      }
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    componentId: {
      required: true,
      type: Number
    },
    alert: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          threshold: 0,
          timespan: 0,
          value: "",
          componentId: 0,
          generalRuleId: 0
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions([
      "createAlert",
      "editAlert",
      "fetchParsingRules",
      "fetchComponents",
      "fetchCustomAttributeRules",
      "fetchMessageRules"
    ]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editAlert(this.alert);
        idForReturn = this.alert.id;
      } else {
        console.log("alert to create");
        console.log(this.alert);
        const newId = await this.createAlert(this.alert);
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(this.alert, this.alertBeforeEdit);
      this.$emit("done", { isEdit: this.isEdit, id: this.alert.id });
    }
  },
  computed: {
    ...mapGetters([
      "customAttributeRuleByComponentId",
      "parsingRuleByComponentId",
      "messageRuleById"
    ]),
    selectableCustomAttributeRules: function() {
      const parsingRules = this.parsingRuleByComponentId(this.componentId);
      let message = this.messageRuleById(parsingRules.messageRuleId);
      message.id = message.generalRuleId;
      const result = this.customAttributeRuleByComponentId(this.componentId);
      result.push(message);
      console.log("selectable");
      console.log(result);
      return result;
    }
  },
  created() {
    this.fetchComponents();
    this.fetchParsingRules();
    this.fetchCustomAttributeRules();
    this.fetchMessageRules();

    if (this.isEdit) {
      this.valid = true;
      Object.assign(this.alertBeforeEdit, this.alert);
    } else {
      this.alertBeforeEdit.componentId = this.componentId;
      Object.assign(this.alert, this.alertBeforeEdit);
    }
  }
};
</script>