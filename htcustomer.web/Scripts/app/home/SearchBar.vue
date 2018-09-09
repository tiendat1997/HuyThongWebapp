<template>
<div class="row">
    <div class="form-group">
        <datepicker :bootstrap-styling="true" :placeholder="'Choose a time'">
        </datepicker>
    </div>
    <div class="form-group mx-2">
        <b-btn v-b-modal.addDeviceModal variant="primary">Nhận đồ sửa</b-btn>        
        <!-- Modal Component --> 
        <b-modal         
          id="addDeviceModal" 
          ref="modal" 
          title="Nhận đồ sửa" 
          size="lg"
          @ok="handleOk" 
          @shown="clearName">    
          <template slot="modal-title">
            <customer-form v-on:updateCustomerInfo="chooseCustomer">
            </customer-form>            
          </template>                   
          <device-form @exit="handleSubmit" v-bind:customer="customer">
          </device-form>
        </b-modal>
    </div>
</div>
</template>

<script>
import Datepicker from "vuejs-datepicker";
import DeviceForm from "./DeviceForm"
import CustomerForm from "./CustomerForm"

export default {
  name: "SearchBar",
  components: {
    DeviceForm,
    Datepicker,  
    CustomerForm
  },
  directives: {  
  },
  data: function() {
    return {
      name: "",
      names: [],        
      customer: null, 
    };
  },
  methods: {
    clearName() {
      this.name = "";
    },
    handleOk(evt) {
      // Prevent modal from closing
      evt.preventDefault();
      if (!this.name) {
        alert("Please enter your name");
      } else {
        this.handleSubmit();
      }
    },
    handleSubmit() {
      this.names.push(this.name);      
      this.clearName();
      this.$refs.modal.hide();
    },  
    chooseCustomer(customer){      
      this.customer = customer;
    }
  }
};
</script>

<style> 
</style>
