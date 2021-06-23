import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import factoryUMLImage from '../assets/factory_uml.png';

const PatternDetails = ({ open, setOpen, patternDetails }) => {
    const toggle = () => setOpen(!open);

    return (
        <Modal isOpen={open} toggle={toggle}>
            <ModalHeader toggle={toggle}>{patternDetails.name}</ModalHeader>
            <ModalBody>
                <div>
                    Factory Method is a creational design pattern that provides an interface for creating objects in a superclass, but allows subclasses to alter the type of objects that will be created.
                </div>
                <img src={factoryUMLImage} width="100%" alt="UML Diagram" style={{ marginTop: 10, marginBottom: 10 }} />
                <div>
                    The Factory Method pattern suggests that you replace direct object construction calls (using the new operator) with calls to a special factory method. Don’t worry: the objects are still created via the new operator, but it’s being called from within the factory method. Objects returned by a factory method are often referred to as products.
                </div>
                <a href="https://refactoring.guru/design-patterns/factory-method" target="blank">Mais informações</a>
            </ModalBody>
            <ModalFooter>
                <Button color="secondary" onClick={toggle}>Close</Button>
            </ModalFooter>
        </Modal>
    );
}

export default PatternDetails;