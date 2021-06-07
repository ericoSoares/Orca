import React, { useState } from 'react';

const Report = ({ analysisResult, setSelectedTab }) => {
    const [selectedFileGroup, setSelectedFileGroup] = useState(null);

    const renderFilesMenu = () => {
        return (
            <nav class="col-md-2 d-none d-md-block bg-light sidebar">
                <div class="sidebar-sticky">
                    <ul class="nav flex-column">
                        <li class="nav-item">
                            <a class="nav-link active" href="#">
                                <span data-feather="home"></span>
                                Overview
                            </a>
                        </li>
                    </ul>
                    <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                        <span>Files</span>
                        <a class="d-flex align-items-center text-muted" href="#">
                            <span data-feather="plus-circle"></span>
                        </a>
                    </h6>
                    <ul class="nav flex-column mb-2">
                        {analysisResult.ruleResultGroups.map(r => (
                            <li class="nav-item">
                                <a class={`nav-link ${selectedFileGroup && selectedFileGroup.filePath === r.filePath && "active"}`}
                                    href="#"
                                    onClick={() => setSelectedFileGroup(r)}
                                >
                                    {r.fileName}
                                </a>
                            </li>    
                        ))}
                    </ul>
                </div>
            </nav>
        );
    }

    const renderList = () => {
        if (!selectedFileGroup) return <div></div>;
        return (
            <div>
                <div class="listHeader mb-4"><h4>File</h4><h2>{selectedFileGroup.filePath}</h2></div>
                {selectedFileGroup.ruleResults.map(r => (
                    <div class="listItem">
                        <div class="itemHeader">
                            <p class="ruleDescription">
                                {r.ruleDescription}
                        </p>
                            <p class="ruleLineNumber">
                                {r.lineNumber}
                        </p>
                        </div>
                        <div class="itemBody">
                            <p class="secondLineInfo">
                                {r.ruleName}, {r.dpName}
                        </p>
                            <p class="severityLevel">{r.severetyLevel}</p>
                        </div>
                    </div>    
                ))}
            </div>
            
        );
    }

    return (
        <div>
            {renderFilesMenu()}
            <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4">
                {renderList()}
            </main>
        </div>
    );
}

export default Report;