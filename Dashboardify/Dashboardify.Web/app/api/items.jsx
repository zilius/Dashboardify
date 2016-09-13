import axios from 'axios';

const rootDomain = `http://${window.location.hostname}/api`;

export default {
  fetchItems (dashboardId) {
    return axios.get(`${rootDomain}/Items/GetList?dashboardId=${dashboardId}`);
  },

  mapBackendData (data) {
    return data.Items.map((item) => {
      return {
        id: item.Id,
        dashboardId: item.DashBoardId,
        name: item.Name,
        img: item.Screenshots.length >= 1 ? 'http://' + window.location.hostname + '/screenshot/'+item.Screenshots[0].ScrnshtURL : '' ,
        url: item.Website,
        isActive: item.IsActive,
        isSelected: false,
        checkInterval: item.CheckInterval,
        created: item.Created,
        lastChecked: item.LastChecked,
        lastModified: item.Modified,
        screenshots: item.Screenshots,
      }
    });
  },

  filterItems (items, dashboardId, searchText) {
    let filteredItems = items;

    filteredItems = filteredItems.filter((item) => {
      return item.dashboardId === dashboardId;
    });

    filteredItems = filteredItems.filter((item) => {
      let containsSearchText = item.name.toLowerCase().indexOf(searchText) !== -1;
      return searchText.length === 0 || containsSearchText;
    });

    return filteredItems;
  },
}
