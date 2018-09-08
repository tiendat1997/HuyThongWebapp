<template>
<div class="row">
    <div class="col-md-4">
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">
            <b-form-group id="categoryGroup" label="Loại thiết bị" label-for="categoryInput">
                <b-form-select id="categoryInput" :options="foods" required v-model="form.category">
                </b-form-select>
            </b-form-group>
            <b-form-group id="errorDescriptionGroup" label="Mô tả lỗi" label-for="errorDescriptionInput">
                <b-form-input id="errorDescriptionInput" type="text" v-model="form.error_desc" required placeholder="Enter Error Description">
                </b-form-input>
            </b-form-group>
            <b-form-group id="deviceDescriptionGroup" label="Mô tả máy" label-for="deviceDescriptionInput">
                <b-form-input id="deviceDescriptionInput" type="text" v-model="form.device_desc" required placeholder="Enter Device Description">
                </b-form-input>
            </b-form-group>
            <b-button type="submit" variant="primary">Thêm</b-button>
            <b-button type="reset" variant="danger">Reset</b-button>
        </b-form>
    </div>
    <div class="col-md-8">
        <label v-if="customer != null">            
            <b>Khách hàng:</b> {{ customer.name }} 
            <span>
                /                    
                {{customer.phone}}
            </span>
        </label>        
        <label v-else>
             <b>Khách hàng:</b> <span style="color:red;">Empty</span>
        </label>
        <b-table striped hover :items="items" :fields="fields">            
            <template slot="action" slot-scope="row">
              <i v-on:click="removeItem(row.item,row.index)" class="fas fa-trash-alt delete-btn"></i>
            </template>           
        </b-table>
    </div>
</div>
</template>

<style>
.delete-btn:hover {
  color: red;
}
</style>

<script>
const fields = [
  {
    key: "action",
    label: "Xóa"
  },
  {
    key: "category",
    label: "Loại"
  },
  {
    key: "error_desc",
    label: "Lỗi"
  },
  {
    key: "device_desc",
    label: "Mô tả"
  }
];

export default {
  name: "DeviceForm",
  components: {},
  props: {
    customer: Object
  },
  data() {
    return {    
      items: [],
      fields: fields,
      form: {
        category: null,
        error_desc: null,
        device_desc: null
      },
      foods: [
        {
          text: "Select One",
          value: null
        },
        "Carrots",
        "Beans",
        "Tomatoes",
        "Corn"
      ],
      show: true
    };
  },
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      // alert(JSON.stringify(this.form));
      this.items.push(this.form);
      this.form = {};
    },
    onReset(evt) {
      evt.preventDefault();
      /* Reset our form values */
      this.form.category = null;
      this.form.error_desc = null;
      this.form.device_desc = null;
      // /* Trick to reset/clear native browser form validation state */
      // this.show = false;
      // this.$nextTick(() => {
      //   this.show = true;
      // });
    },
    removeItem(item, index) {
      this.items.splice(index, 1);
    }
  }
};
</script>
