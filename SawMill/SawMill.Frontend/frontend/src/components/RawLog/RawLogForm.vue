<template>
  <v-form ref="form">
    <v-textarea v-if="!isFileInput" filled label="Log" v-model="lines" auto-grow></v-textarea>
    <v-file-input v-if="isFileInput" v-model="file" show-size counter label="Log File Input"></v-file-input>
    <v-btn text @click="isFileInput = !isFileInput">Switch Input</v-btn>

    <v-btn @click="submit">submit</v-btn>
  </v-form>
</template>



<script>
export default {
  name: "raw-log-form",
  props: {
    componentId: {
      type: Number,
      required: true,
      default: 0
    }
  },
  data: () => {
    return {
      isFileInput: false,
      valid: true,
      file: [],
      lines: ""
    };
  },
  methods: {
    cancel: function() {
      this.$router.go(-1);
    },
    submit: async function() {
      try {
        if (this.isFileInput) {
          let formData = new FormData();
          formData.append("logFile", this.file);
          const response = await this.$http.post(
            `log/raw/file/${this.componentId}`,
            formData,
            {
              headers: {
                "Content-Type": "multipart/form-data"
              }
            }
          );
          console.log(response.data);
        } else {
          const lines = this.lines.split(/\r?\n/);
          console.log("submiting lines");
          console.log(lines);
          const response = await this.$http.post(
            `log/raw/${this.componentId}`,
            lines
          );
          console.log(response.data);
        }
      } catch (error) {
        // TODO: how to handle error, show to user?
        console.log(error);
      }
    }
  }
};
</script>

<style>
</style>