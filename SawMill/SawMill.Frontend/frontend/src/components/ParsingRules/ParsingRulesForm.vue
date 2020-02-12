<template>
  <v-stepper v-model="currentStep">
    <v-stepper-header>
      <v-stepper-step
        :complete="parsingRules.dateTimeRuleId > 0"
        editable
        :step="steps.dateTimeStep.index"
      >Create Date Time Rule</v-stepper-step>

      <v-stepper-step
        :complete="parsingRules.messageRuleId > 0"
        editable
        :step="steps.messageStep.index"
      >Create Message Rule</v-stepper-step>

      <v-stepper-step
        :complete="parsingRules.severityRuleId > 0"
        editable
        :step="steps.severityStep.index"
      >Create Severity Rule</v-stepper-step>

      <v-stepper-step
        :complete="parsingRules.customAttributeRuleIds.length > 0"
        editable
        :step="steps.customAttributeStep.index"
      >Create Custom Attribute Rules</v-stepper-step>

      <v-progress-linear
        :active="loading"
        :indeterminate="loading"
        absolute
        bottom
        color="deep-purple accent-4"
      ></v-progress-linear>
    </v-stepper-header>

    <v-stepper-items>
      <v-stepper-content :step="steps.dateTimeStep.index">
        <v-row>
          <v-col cols="12" sm="8" md="8" lg="8">
            <v-select
              v-model="parsingRules.dateTimeRuleId"
              :items="selectDateTime"
              item-text="name"
              item-value="id"
              label="Date Time Rule"
            ></v-select>
          </v-col>
          <v-col cols="12" sm="4" md="4" lg="4">
            <v-btn text icon @click="isDateTimeCreate = true">
              <v-icon>mdi-plus-thick</v-icon>
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <dateTime-rule
              v-if="!isDateTimeCreate && parsingRules.dateTimeRuleId != 0"
              :dateTimeRuleId="parsingRules.dateTimeRuleId"
            ></dateTime-rule>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <dateTimeRule-form
              v-if="isDateTimeCreate"
              v-on:done="dateTimeCreateDone"
              :isEdit="this.isDateTimeEdit"
              :dateTimeRule="this.dateTimeRuleById(this.parsingRules.dateTimeRuleId)"
            ></dateTimeRule-form>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="4" md="4" lg="4" class="text-left">
            <v-btn
              v-if="!isDateTimeCreate && parsingRules.dateTimeRuleId != 0"
              color="primary"
              @click="() => {this.isDateTimeEdit = true; this.isDateTimeCreate = true}"
            >Edit</v-btn>
          </v-col>
          <v-col cols="12" sm="8" md="8" lg="8" class="text-right">
            <v-btn color="primary" @click="currentStep = steps.messageStep.index">Continue</v-btn>

            <v-btn text @click="cancel">Cancel</v-btn>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <v-form>
              <v-text-field
                v-model="inputTestDateTime"
                label="Input log line on which you whish to test out current rule"
              ></v-text-field>
              <v-text-field :value="parseResultDateTime" label="Result" outlined readonly></v-text-field>
              <v-btn color="grey" class="mr-4" @click="parseDateTime">Test Rule</v-btn>
            </v-form>
          </v-col>
        </v-row>
      </v-stepper-content>

      <v-stepper-content :step="steps.messageStep.index">
        <v-row>
          <v-col cols="12" sm="8" md="8" lg="8">
            <v-select
              v-model="parsingRules.messageRuleId"
              :items="selectMessage"
              item-text="name"
              item-value="id"
              label="Message Rule"
            ></v-select>
          </v-col>
          <v-col cols="12" sm="4" md="4" lg="4">
            <v-btn text icon @click="isMessageCreate = true">
              <v-icon>mdi-plus-thick</v-icon>
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <message-rule
              v-if="!isMessageCreate && parsingRules.messageRuleId != 0"
              :messageRuleId="parsingRules.messageRuleId"
            ></message-rule>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <messageRule-form
              v-if="isMessageCreate"
              :isEdit="isMessageEdit"
              :messageRule="this.messageRuleById(this.parsingRules.messageRuleId)"
              v-on:done="messageCreateDone"
            ></messageRule-form>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="4" md="4" lg="4" class="text-left">
            <v-btn
              v-if="!isMessageCreate && parsingRules.messageRuleId != 0"
              color="primary"
              @click="() => {isMessageEdit = true; isMessageCreate = true}"
            >Edit</v-btn>
          </v-col>
          <v-col cols="12" sm="8" md="8" lg="8" class="text-right">
            <v-btn color="primary" @click="currentStep = steps.severityStep.index">Continue</v-btn>

            <v-btn text @click="cancel">Cancel</v-btn>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <v-form>
              <v-text-field
                v-model="inputTestMessage"
                label="Input log line on which you whish to test out current rule"
              ></v-text-field>
              <v-text-field :value="parseResultMessage" label="Result" outlined readonly></v-text-field>
              <v-btn color="grey" class="mr-4" @click="parseMessage">Test Rule</v-btn>
            </v-form>
          </v-col>
        </v-row>
      </v-stepper-content>

      <v-stepper-content :step="steps.severityStep.index">
        <v-row>
          <v-col cols="12" sm="8" md="8" lg="8">
            <v-select
              v-model="parsingRules.severityRuleId"
              :items="selectSeverity"
              item-text="name"
              item-value="id"
              label="Severity Rule"
            ></v-select>
          </v-col>
          <v-col cols="12" sm="4" md="4" lg="4">
            <v-btn text icon @click="isSeverityCreate = true">
              <v-icon>mdi-plus-thick</v-icon>
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <severity-rule
              v-if="!isSeverityCreate && parsingRules.severityRuleId != 0"
              :severityRuleId="parsingRules.severityRuleId"
            ></severity-rule>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <severityRule-form
              v-if="isSeverityCreate"
              v-on:done="severityCreateDone"
              :isEdit="isSeverityEdit"
              :severityRule="this.severityRuleById(this.parsingRules.severityRuleId)"
            ></severityRule-form>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" sm="4" md="4" lg="4" class="text-left">
            <v-btn
              v-if="!isSeverityCreate && parsingRules.severityRuleId != 0"
              color="primary"
              @click="() => {isSeverityEdit = true; isSeverityCreate = true}"
            >Edit</v-btn>
          </v-col>
          <v-col cols="12" sm="8" md="8" lg="8" class="text-right">
            <v-btn
              color="primary"
              v-on:click="currentStep = steps.customAttributeStep.index"
            >Continue</v-btn>

            <v-btn text @click="cancel">Cancel</v-btn>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <v-form>
              <v-text-field
                v-model="inputTestSeverity"
                label="Input log line on which you whish to test out current rule"
              ></v-text-field>
              <v-text-field :value="parseResultSeverity" label="Result" outlined readonly></v-text-field>
              <v-btn color="grey" class="mr-4" @click="parseSeverity">Test Rule</v-btn>
            </v-form>
          </v-col>
        </v-row>
      </v-stepper-content>

      <v-stepper-content :step="steps.customAttributeStep.index">
        <v-row>
          <v-col cols="12" sm="10" md="10" lg="10">
            <v-select
              v-model="parsingRules.customAttributeRuleIds"
              :items="selectCustomAttribute"
              item-text="name"
              item-value="id"
              label="Custom attribute Rules"
              multiple
            ></v-select>
          </v-col>
          <v-col cols="12" sm="2" md="2" lg="2">
            <v-btn text icon @click="isCustomAttributeCreate = true">
              <v-icon>mdi-plus-thick</v-icon>
            </v-btn>
          </v-col>
        </v-row>

        <v-row v-if="isCustomAttributeCreate">
          <v-col cols="12" sm="12" md="12" lg="12">
            <customAttributeRule-form v-on:done="isCustomAttributeCreate = false"></customAttributeRule-form>
          </v-col>
        </v-row>

        <v-row
          v-for="customAttributeId in parsingRules.customAttributeRuleIds"
          :key="customAttributeId"
        >
          <v-col cols="12" sm="12" md="12" lg="12">
            <custom-attribute-rule
              v-if="!isCustomAttributeRuleEdit(customAttributeId)"
              :customAttributeRuleId="customAttributeId"
            ></custom-attribute-rule>
            <customAttributeRule-form
              v-if="isCustomAttributeRuleEdit(customAttributeId)"
              :isEdit="true"
              :customAttributeRule="customAttributeRuleById(customAttributeId)"
              v-on:done="switchCustomAttributeRuleEdit(customAttributeId)"
            ></customAttributeRule-form>
          </v-col>
          <v-col cols="12" sm="4" md="4" lg="4" class="text-left">
            <v-btn color="primary" @click="switchCustomAttributeRuleEdit(customAttributeId)">Edit</v-btn>
          </v-col>
          <v-col cols="12" sm="12" md="12" lg="12">
            <v-form>
              <v-text-field
                v-model="inputTestCustomAttribute[customAttributeId]"
                label="Input log line on which you whish to test out current rule"
              ></v-text-field>
              <v-text-field
                :value="parseResultCustomAttribute[customAttributeId]"
                label="Result"
                outlined
                readonly
              ></v-text-field>
              <v-btn
                color="grey"
                class="mr-4"
                @click="parseCustomAttribute(customAttributeId)"
              >Test Rule</v-btn>
            </v-form>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" sm="8" md="8" lg="8" class="text-right">
            <v-btn color="primary" v-on:click="submit">Complete Setup</v-btn>

            <v-btn text @click="cancel">Cancel</v-btn>
          </v-col>
        </v-row>

        <v-row></v-row>
      </v-stepper-content>
    </v-stepper-items>
  </v-stepper>
</template>

<script>
import dateTimeRuleForm from "../DateTimeRule/DateTimeRuleForm";
import dateTimeRule from "../DateTimeRule/DateTimeRule";
import messageRuleForm from "../MessageRule/MessageRuleForm";
import messageRule from "../MessageRule/MessageRule";
import severityRuleForm from "../SeverityRule/SeverityRuleForm";
import severityRule from "../SeverityRule/SeverityRule";
import customAttributeRule from "../CustomAttributeRule/CustomAttributeRule";
import customAttributeRuleForm from "../CustomAttributeRule/CustomAttributeRuleForm";
import { mapGetters, mapActions } from "vuex";
export default {
  name: "parsingRules-form",
  props: {
    isEdit: {
      default: false,
      type: Boolean,
      required: false
    },
    forComponent: {
      required: true,
      type: Number
    },
    parsingRuleId: {
      default: 0,
      type: Number,
      required: false
    }
  },
  data: () => {
    return {
      parseResultDateTime: "",
      parseResultMessage: "",
      parseResultSeverity: "",
      parseResultCustomAttribute: {},
      inputTestDateTime: "",
      inputTestMessage: "",
      inputTestSeverity: "",
      inputTestCustomAttribute: {},
      loading: false,
      steps: {
        dateTimeStep: { isComplete: false, index: 1 },
        messageStep: { isComplete: false, index: 2 },
        severityStep: { isComplete: false, index: 3 },
        customAttributeStep: { isComplete: false, index: 4 }
      },
      currentStep: 1,
      isDateTimeCreate: false,
      isDateTimeEdit: false,
      isMessageCreate: false,
      isMessageEdit: false,
      isSeverityCreate: false,
      isSeverityEdit: false,
      isCustomAttributeCreate: false,
      customAttributeRuleEdits: [],
      parsingRules: {
        id: 0,
        componentId: 0,
        dateTimeRuleId: 0,
        messageRuleId: 0,
        severityRuleId: 0,
        customAttributeRuleIds: []
      }
    };
  },
  methods: {
    ...mapActions([
      "fetchDateTimeRules",
      "fetchSeverityRules",
      "fetchMessageRules",
      "fetchCustomAttributeRules",
      "fetchParsingRule",
      "createParsingRule",
      "editParsingRule"
    ]),
    cancel: function() {
      this.$router.go(-1);
    },
    isCustomAttributeRuleEdit: function(customAttributeRuleId) {
      return this.customAttributeRuleEdits.includes(customAttributeRuleId);
    },
    switchCustomAttributeRuleEdit: function(customAttributeRuleId) {
      if (!this.customAttributeRuleEdits.includes(customAttributeRuleId)) {
        this.customAttributeRuleEdits.unshift(customAttributeRuleId);
      } else {
        const idIndex = this.customAttributeRuleEdits.indexOf(
          customAttributeRuleId
        );
        if (idIndex > -1) {
          this.customAttributeRuleEdits.splice(idIndex, 1);
        }
      }
    },
    dateTimeCreateDone: function({ isEdit, id }) {
      this.parsingRules.dateTimeRuleId = id;
      this.isDateTimeCreate = false;
      this.isDateTimeEdit = false;
    },
    severityCreateDone: function({ isEdit, id }) {
      this.parsingRules.severityRuleId = id;
      this.isSeverityCreate = false;
      this.isSeverityEdit = false;
    },
    messageCreateDone: function({ isEdit, id }) {
      this.parsingRules.messageRuleId = id;
      this.isMessageCreate = false;
      this.isMessageEdit = false;
    },
    submit: async function() {
      this.loading = true;
      console.log(this.parsingRules);

      if (this.isEdit) {
        await this.editParsingRule(this.parsingRules);
      } else {
        await this.createParsingRule(this.parsingRules);
      }
      this.loading = false;
      this.$router.go(-1);
    },
    parseDateTime: async function() {
      this.loading = true;
      let dateTimeRule = this.dateTimeRuleById(
        this.parsingRules.dateTimeRuleId
      );
      let result = await this.$http.post("parse/datetime", {
        line: this.inputTestDateTime,
        ruleViewModel: dateTimeRule
      });
      this.parseResultDateTime = result.data;
      this.loading = false;
    },
    parseSeverity: async function() {
      this.loading = true;
      let severityRule = this.severityRuleById(
        this.parsingRules.severityRuleId
      );
      let result = await this.$http.post("parse/severity", {
        line: this.inputTestSeverity,
        ruleViewModel: severityRule
      });
      this.parseResultSeverity = result.data.display;
      console.log(result.data);
      this.loading = false;
    },
    parseMessage: async function() {
      this.loading = true;
      let messageRule = this.messageRuleById(this.parsingRules.messageRuleId);
      let result = await this.$http.post("parse/message", {
        line: this.inputTestMessage,
        ruleViewModel: messageRule
      });
      this.parseResultMessage = result.data;
      this.loading = false;
    },
    parseCustomAttribute: async function(customAttributeId) {
      this.loading = true;
      let customAttributeRule = this.customAttributeRuleById(customAttributeId);
      let result = await this.$http.post("parse/customattribute", {
        line: this.inputTestCustomAttribute[customAttributeId],
        ruleViewModel: customAttributeRule
      });
      this.parseResultCustomAttribute[customAttributeId] = result.data;
      this.loading = false;
    }
  },
  computed: {
    ...mapGetters([
      "allDateTimeRules",
      "allSeverityRules",
      "allMessageRules",
      "allCustomAttributeRules",
      "dateTimeRuleById",
      "messageRuleById",
      "severityRuleById",
      "customAttributeRuleById",
      "parsingRuleById"
    ]),
    selectDateTime: function() {
      return this.allDateTimeRules;
    },
    selectMessage: function() {
      return this.allMessageRules;
    },
    selectSeverity: function() {
      return this.allSeverityRules;
    },
    selectCustomAttribute: function() {
      return this.allCustomAttributeRules;
    }
  },
  created() {
    if (this.isEdit) {
      console.log("edit mode for parsing rules");
      this.fetchParsingRule(this.parsingRuleId);
    } else {
      this.parsingRules.componentId = this.forComponent;
    }
    this.fetchCustomAttributeRules();
    this.fetchDateTimeRules();
    this.fetchMessageRules();
    this.fetchSeverityRules();
  },
  mounted() {
    this.loading = true;
    if (this.isEdit) {
      while (typeof this.parsingRuleById(this.parsingRuleId) === undefined) {}

      this.parsingRules = this.parsingRuleById(this.parsingRuleId);
    }
    this.loading = false;
  },
  components: {
    dateTimeRuleForm,
    dateTimeRule,
    messageRuleForm,
    messageRule,
    severityRuleForm,
    severityRule,
    customAttributeRule,
    customAttributeRuleForm
  }
};
</script>