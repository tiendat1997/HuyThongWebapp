<template>
<div class="row">
    <div class="col-md-4">
        <b-form @submit="onSubmitAddItem" @reset="onResetAddItem" v-if="show">
            <b-form-group id="categoryGroup" label="Loại thiết bị" label-for="categoryInput">
                <b-form-select id="categoryInput" :options="categoryOptions" required v-model="form.CategoryId">
                    <template slot="first">
                        <!-- this slot appears above the options from 'options' prop -->
                        <option :value="null" disabled>-- Chọn loại --</option>
                    </template>
                </b-form-select>
            </b-form-group>
            <b-form-group id="errorDescriptionGroup" label="Mô tả lỗi" label-for="errorDescriptionInput">
                <b-form-input id="errorDescriptionInput" type="text" v-model="form.Error" required placeholder="Enter Error Description">
                </b-form-input>
            </b-form-group>
            <b-form-group id="deviceDescriptionGroup" label="Mô tả máy" label-for="deviceDescriptionInput">
                <b-form-input id="deviceDescriptionInput" type="text" v-model="form.DeviceDescription" required placeholder="Enter Device Description">
                </b-form-input>
            </b-form-group>
            <b-button type="submit" variant="primary">Thêm</b-button>
            <b-button type="reset" variant="danger">Reset</b-button>
        </b-form>
    </div>
    <div class="col-md-8">
        <label v-if="customer != null">            
            <b>Khách hàng:</b> {{ customer.Name }} 
            <span>
                /                    
                {{customer.Phone}}
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
const fields = [{
        key: "action",
        label: "Xóa"
    },
    {
        key: "category_name",
        label: "Loại"
    },
    {
        key: "Error",
        label: "Lỗi"
    },
    {
        key: "DeviceDescription",
        label: "Mô tả"
    }
];

export default {
    name: "DeviceForm",
    components: {},
    props: {
        customer: Object, 
        items: Array
    },
    data() {
        return {          
            fields: fields,
            categories: [],
            form: {
                CategoryId: null,
                category_name: null,
                Error: null,
                DeviceDescription: null
            },
            categoryOptions: [],
            show: true
        };
    },
    mounted: function () {
        this.$nextTick(function () {
            // load list category
            let api = "/category/getlistcategory";
            this.axios.get(api).then(response => {      
                this.categories = response.data;
                response.data.forEach((item) => {
                  this.categoryOptions.push({
                    text: item.Name,
                    value: item.CategoryID
                  });
                });
            });
        });
    },
    methods: {
        onSubmitAddItem(evt) {
            evt.preventDefault();            
            this.categoryOptions.forEach((item) => {                                 
              if (item.value === this.form.CategoryId) {
                this.form.category_name = item.text;                
              }
            });
            console.log(this.form);
            // alert(JSON.stringify(this.form));
            this.items.push(this.form);
            this.form = {};
        },
        onResetAddItem(evt) {
            evt.preventDefault();
            /* Reset our form values */
            this.form.CategoryId = null;
            this.form.Error = null;
            this.form.DeviceDescription = null;      
        },
        removeItem(item, index) {
            this.items.splice(index, 1);
        }
    }
};
</script>
