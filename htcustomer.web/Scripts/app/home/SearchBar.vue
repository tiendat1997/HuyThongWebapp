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
          @ok="handleOk">     <!-- @shown="resetModal" -->           
          <template slot="modal-title">
            <customer-form @updateCustomerInfo="chooseCustomer"> 
              <!-- @ shorthand : v-on:function -->
            </customer-form>            
          </template>                   
          <device-form @exit="handleSubmit" :customer="customer" :items="items">
            <!-- -v-bind:customer = :customer -->
          </device-form>
        </b-modal>
    </div>
</div>
</template>

<script>
const JSON_STATUS = {
  Success: "Success",
  Unvalidated: "Unvalidated",
  Fail: "Failure"
};
import Datepicker from "vuejs-datepicker";
import DeviceForm from "./DeviceForm";
import CustomerForm from "./CustomerForm";

export default {
  name: "SearchBar",
  components: {
    DeviceForm,
    Datepicker,
    CustomerForm
  },
  directives: {},
  data: function() {
    return {      
      items: [], // transaction list
      customer: null
    };
  },
  methods: {
    resetModal() {
        this.items = []; 
        this.customer = null; 
    },
    handleOk(evt) {
      // Prevent modal from closing
      evt.preventDefault();
      console.log(this.customer);
      console.log(this.items);
      if (this.customer === null) {
        alert("Vui lòng chọn thông tin khách hàng !");
      } else if (this.items.length == 0) {
        alert("Vui lòng thêm đồ sửa !");
      } else {
        this.handleSubmit();
      }
    },
    handleSubmit() {
      var api = "/device/AddTransactions";
      var self = this;
      this.axios
        .post(api, {
          CustomerId: this.customer.CustomerId,
          Transactions: this.items
        })
        .then(function(response) {          
          let status = response.data.Status;
          if (status === JSON_STATUS.Success) {            
            alert(response.data.Message);
            self.$refs.modal.hide();
            self.resetModal();
          } else if (status === JSON_STATUS.Fail) {
            alert(response.data.Message);
          }
        });          
    },
    chooseCustomer(customer) {
      this.customer = customer;
    }
  }
};
</script>

<style>
</style>
