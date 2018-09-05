<template>  
  <div class="row">       
			<Container 
				orientation="horizontal" 
				@drop="onColumnDrop($event)"
				drag-handle-selector=".column-drag-handle"
				@drag-start="dragStart"
        class="row"
			>
				<Draggable 
          v-for="column in scene.children" 
          :key="column.id"
          class="col-md-4 col-xs-12">
					<div :class="column.props.className">
						<div class="card-column-header card-header bg-primary">
							<span class="column-drag-handle" style="color: white">&#x2630;</span>
							<span style="color: white">{{column.name}}</span>
						</div>
						<Container 
							group-name="col"
							@drop="(e) => onCardDrop(column.id, e)"
              @drag-start="(e) => log('drag start', e)"
              @drag-end="(e) => log('drag end', e)"
							:get-child-payload="getCardPayload(column.id)"
							drag-class="card-ghost"
							drop-class="card-ghost-drop"            
						>          
							<Draggable v-for="card in column.children" 
                :key="card.id">
								<div :class="card.props.className" :style="card.props.style">
                  <div class="card-header">Header</div>
                  <div class="card-body">
                    <p class="card-text">
                      {{card.data}}
                    </p>
                  </div>									
								</div>
							</Draggable>
						</Container>
					</div>
				</Draggable>
			</Container>
    </div>
</template>

<script>
const lorem = `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.`;
const columnNames = ["Chưa sửa (n)", "Đã sửa (n)", "Không thể sửa (n)"];
const cardColors = [
  "azure",
  "beige",
  "bisque",
  "blanchedalmond",
  "burlywood",
  "cornsilk",
  "gainsboro",
  "ghostwhite",
  "ivory",
  "khaki"
];

const pickColor = () => {
  const rand = Math.floor(Math.random() * 10);
  return cardColors[rand];
};
const scene = {
  type: "container",
  props: {
    orientation: "horizontal"
  },
  children: generateItems(3, i => ({
    id: `column${i}`,
    type: "container",
    name: columnNames[i],
    props: {
      orientation: "vertical",
      className: "card-container"
    },
    children: generateItems(+(Math.random() * 10).toFixed() + 5, j => ({
      type: "draggable",
      id: `${i}${j}`,
      props: {
        className: "card border-danger",
        style: { backgroundColor: pickColor() }
      },
      data: lorem.slice(0, Math.floor(Math.random() * 150) + 30)
    }))
  }))
};
import { Container, Draggable } from "vue-smooth-dnd";
import { applyDrag, generateItems } from "./utils";

export default {
  name: "DeviceView",
  components: {
    Container,
    Draggable,  
  },
  data() {
    return {
      dismissSecs: 5,
      dismissCountDown: 0,
      showDismissibleAlert: false,
      scene
    };
  },
  methods: {
    onColumnDrop: function(dropResult) {
      const scene = Object.assign({}, this.scene);
      scene.children = applyDrag(scene.children, dropResult);
      this.scene = scene;
    },
    onCardDrop: function(columnId, dropResult) {
      if (dropResult.removedIndex !== null || dropResult.addedIndex !== null) {
        const scene = Object.assign({}, this.scene);
        const column = scene.children.filter(p => p.id === columnId)[0];
        const columnIndex = scene.children.indexOf(column);
        const newColumn = Object.assign({}, column);
        newColumn.children = applyDrag(newColumn.children, dropResult);
        scene.children.splice(columnIndex, 1, newColumn);
        this.scene = scene;
      }
    },
    getCardPayload: function(columnId) {
      return index => {
        return this.scene.children.filter(p => p.id === columnId)[0].children[
          index
        ];
      };
    },
    dragStart: function() {
      console.log("drag started");
    },
    log: function(...params) {
      console.log(...params);
    }
  }
};
</script>

<style>
.card-ghost {
  opacity: 0.5;
}
</style>