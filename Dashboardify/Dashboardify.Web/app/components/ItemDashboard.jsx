import React from 'react';

import ItemList from 'ItemList';
import ItemPanel from 'ItemPanel';
import DashboardifyAPI from 'DashboardifyAPI';

class ItemDashboard extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            items: DashboardifyAPI.getItems(this.props.params.id),

            selectedItemId: undefined

        }
    }

    handleToggleItem(id) {
        let updatedItems = this.state.items.map((item) => {
            if (item.id === id) {
                item.isActive = !item.isActive;
            }

            return item;
        });

        this.setState({
            items: updatedItems
        })
    }

    handleItemClick(id) {
        let updatedItems = this.state.items.map((item) => {
            if (item.id === id) {
                item.isSelected = true;
            } else {
                item.isSelected = false;
            }

            return item;
        });

        this.setState({
            selectedItemId: id,
            items: updatedItems
        });
    }

    render() {

        let {items} = this.state;
        let getSelectedItem = (id) => {
            let itemId  = items.findIndex((item) => {
                if (item.id === id) {
                    return item;
                }
            });

            return itemId;
        }

        let renderItemPanel = () => {
            if (typeof this.state.selectedItemId === 'number') {
                return (
                    <ItemPanel item={items[getSelectedItem(this.state.selectedItemId)]} toggleItem={this.handleToggleItem.bind(this)}/>
                )
            }
        }


        //Needs refactoring
        return (
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-6 col-lg-8">
                        <div className="row">
                            <div className="col-md-6">
                                <ol className="breadcrumb">
                                    <li>
                                        <a href="#">Home</a>
                                    </li>
                                    <li className="active">
                                        <a href="#">Audi</a>
                                    </li>
                                </ol>
                            </div>
                            <div className="col-md-6">
                                <div className="input-group">
                                    <input type="text" className="form-control" placeholder="Search for..."/>
                                        <span className="input-group-btn">
                                            <button className="btn btn-default" type="button">Go!</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <hr/>
                            <ItemList items={items} itemClick={this.handleItemClick.bind(this)}/>
                        </div>
                        <div className="col-md-6 col-lg-4">
                            {renderItemPanel()}
                        </div>
                    </div>
                </div>

            );
        }
    }

    export default ItemDashboard;
