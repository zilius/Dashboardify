export let addDashboards = (dashboards) => {
  return {
    type: 'ADD_DASHBOARDS',
    dashboards
  };
};

export let setSearchText = (searchText) => {
  return {
    type: 'SET_SEARCH_TEXT',
    searchText
  };
};

export let addItems = (items) => {
  return {
    type: 'ADD_ITEMS',
    items
  }
}
