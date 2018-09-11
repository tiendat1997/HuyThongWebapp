<template>
<div class="row">
    <div class="form col-md-4">
        <div class="form-group">
            <label class="form-title" for="customer-filter">Tìm khách hàng</label>
            <v-autocomplete id="customer-filter" :input-class="'form-control mb-2 mr-sm-2'" :items="items" v-model="item" :get-label="getLabel" :auto-select-one-item="false" :placeholder="'Gõ ít nhất 3 kí tự để tìm'" :component-item='template' @update-items="updateItems"
                @item-selected="chooseItem">
            </v-autocomplete>
        </div>
    </div>
    <div class="col-md-8">
        <label class="form-title">Thêm khách hàng            
        </label>
        <b-form inline @submit="onAddCustomer" @reset="onResetCustomerForm">
            <b-form-input id="new-customer-name" type="text" v-model="newCustomer.Name" required class="form-control mb-2 mr-sm-2" placeholder="Tên khách hàng">
            </b-form-input>
            <b-form-input id="new-customer-phone" type="tel" v-model="newCustomer.Phone" class="form-control mb-2 mr-sm-2" required placeholder="Điện thoại" 
            pattern="^\d{10,}$" title="Số điện thoại không hợp lệ">
            </b-form-input>
            <b-button type="submit" class="btn btn-sm btn-primary mb-2" variant="primary">Thêm</b-button>                        
        </b-form>
    </div>
</div>
</template>

<script>
const JSON_STATUS = {
  Success: "Success",
  Unvalidated: "Unvalidated"
};

import CustomerView from "./CustomerView";
import Autocomplete from "v-autocomplete";

export default {
  name: "CustomerForm",
  components: {
    CustomerView,
    "v-autocomplete": Autocomplete
  },
  data() {
    return {
      item: null,
      items: [],
      newCustomer: {
        Name: "",
        Phone: ""
      },
      template: CustomerView
    };
  },
  methods: {
    onAddCustomer(evt) {
      evt.preventDefault();
      var api = "/contact/addcustomer";
      var self = this; 
      this.axios
        .post(api, this.newCustomer)
        .then(function(response) {
          let status = response.data.Status;
          if (status === JSON_STATUS.Success) {
            alert(response.data.Message);                        
            self.$emit("updateCustomerInfo", self.newCustomer);
          } else if (status === JSON_STATUS.Unvalidated) {            
            alert(response.data.Message);
          }
        })
        .catch(function(error) {
          console.log(error);
          alert(error);
        });
    },
    onResetCustomerForm() {},
    getLabel(item) {
      if (item === null) return;
      return item.CustomerName;
    },
    updateItems(text) {
      let api = `/contact/searchcustomer`;
      var self = this;
      this.axios
        .get(api, {
          params: {
            searchvalue: text
          }
        })
        .then(response => {
          this.items = response.data;
        });
    },
    chooseItem(item) {
      this.$emit("updateCustomerInfo", item);
    }
  }
};
</script>

<style>
#addDeviceModal .modal-title {
  width: 100%;
}

.v-autocomplete {
  position: relative;
}

.v-autocomplete .v-autocomplete-list {
  position: absolute;
  width: 100%;
  background-color: #eee;
  z-index: 1000;
}

.v-autocomplete .v-autocomplete-list .v-autocomplete-list-item {
  cursor: pointer;
  font-size: 15px;
}

.v-autocomplete
  .v-autocomplete-list
  .v-autocomplete-list-item.v-autocomplete-item-active {
  background-color: #c2c7cc;
}
</style>
