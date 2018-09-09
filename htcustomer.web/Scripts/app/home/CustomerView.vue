<template>
<div class="col-md-12">
    <p>{{ item.CustomerName }}</p>
</div>
</template>

<script>
export default {
    name: "CustomerView",
    props: {
        item: {
            required: true
        },
        searchText: {
            required: true
        }
    }
};
</script>
