<template>
<div class="row">
    <div class="form col-md-4">
        <div class="form-group">        
            <label class="form-title" for="customer-filter">Tìm khách hàng</label>
            <v-autocomplete 
                id="customer-filter"
                :input-class="'form-control mb-2 mr-sm-2'"
                :items="items" v-model="item" :get-label="getLabel"
                :auto-select-one-item="false"
                :placeholder="'Gõ ít nhất 3 kí tự để tìm'" 
                :component-item='template' 
                @update-items="updateItems"                
                @item-selected="chooseItem">
            </v-autocomplete>            
        </div>        
    </div> 
    <div class="col-md-8">       
        <label class="form-title">Thêm khách hàng            
        </label>                        
    <form class="form-inline"> 
        <input type="text" class="form-control mb-2 mr-sm-2" id="new-customer-name" placeholder="Tên khách hàng">                   
        <input type="phone" class="form-control mb-2 mr-sm-2" id="new-customer-phone" placeholder="Điện thoại">                    
        <button type="submit" class="btn btn-sm btn-primary mb-2">Thêm</button>    
    </form>
    </div>   
</div>
</template>

<script>
const customers = [
  {
    id: 1,
    name: "Trần Tiến Đạt",
    phone: "01643734810"
  },
  {
    id: 2,
    name: "Nguyễn Thúy Ngọc",
    phone: "01218351464"
  },
  {
    id: 3,
    name: "Trần Thanh Huy",
    phone: "0913952190"
  },
  {
    id: 4,
    name: "Nguyễn Thị Bạch Tuyết",
    phone: "0994491609"
  },
  {
    id: 5,
    name: "Nguyễn Ngọc Thúy",
    phone: "01218351464"
  },
  {
    id: 6,
    name: "Ngọc Thúy Nguyễn",
    phone: "01218351464"
  }
];
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
      template: CustomerView
    };
  },
  methods: {
    getLabel(item) {
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
